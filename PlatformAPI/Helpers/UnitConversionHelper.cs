
using PlatformAPI.Models.Recipe;
using PlatformAPI.Models.UnitConversion;
using PlatformAPI.Enums;
using PlatformAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace PlatformAPI.Helpers
{

    public class IngredientUnitList
    {
        public string Ingredient { get; set; }

        public string NormalizedIngredient { get; set; }

        public string BaseUnit { get; set; }

        public string TargetUnit { get; set; }

        public string TargetUnitAlternative { get; set; }

        public float? Quantity {  get; set; }

        public float? QuantityMax { get; set; }

        public MeasurementSystem TargetSystem { get; set; }

        public float ConversionFactor { get; set; }

        public float? TargetQuantity { get; set; }

        public float? TargetQuantityMax {  get; set; }

        public string? FinalQuantity { get; set; }

        public string? FinalQuantityMax { get; set; }

        public bool Convert { get; set; }

        public IngredientType? IngredientType { get; set; }

        public int? SortOrder { get; set; }
        public bool IsActive { get; set; }

        
    }
    public class UnitConversionHelper
    {

        private readonly DbContext _context;

        public UnitConversionHelper(DbContext context)
        {
            _context = context;
        }

        public async Task<List<IngredientUnit>> LoadIngredientUnitTableAsync()
        {
            return await _context.Set<IngredientUnit>().ToListAsync();
        }

        public async Task<List<UnitConversion>> LoadUnitConversionTableAsync()
        {
            return await _context.Set<UnitConversion>().ToListAsync();
        }

        public async Task<List<UnitConversionException>> LoadUnitConversionExceptionTableAsync()
        {
            return await _context.Set<UnitConversionException>().ToListAsync();
        }

        public async Task<List<UnitPrecisionRule>> LoadUnitPrecisionRuleTableAsync()
        {
            return await _context.Set<UnitPrecisionRule>().ToListAsync();
        }

        public async Task<List<ImperialRoundingRule>> LoadImperialRoundingRuleTableAsync()
        {
            return await _context.Set<ImperialRoundingRule>().ToListAsync();
        }

        //public async Task<List<IngredientUnitList>> GetConvertedUnit(List<Ingredient> Ingredients, MeasurementSystem measurementSystem)
        public async Task<List<Ingredient>> GetConvertedUnit(List<Ingredient> Ingredients, MeasurementSystem measurementSystem)
        {
            List<IngredientUnitList> subList = new List<IngredientUnitList>();
            List<Ingredient> newList = new List<Ingredient>();

            var unitDB = new UnitDB(_context);
            List<Unit> unitTable = await unitDB.LoadUnitTableAsync();

            List<IngredientUnit> ingredientUnitTable =
                    await LoadIngredientUnitTableAsync();

            List<UnitConversion> unitConversionTable =
                    await LoadUnitConversionTableAsync();

            List<UnitConversionException> unitConversionExceptionTable =
                await LoadUnitConversionExceptionTableAsync();

            List<UnitPrecisionRule> unitPrecisionRuleTable =
                await LoadUnitPrecisionRuleTableAsync();

            List<ImperialRoundingRule> imperialRoundingRuleTable =
                await LoadImperialRoundingRuleTableAsync();

            //Step 1.  Traverse IngredientUnits table.  Find all ingredients and map to 
            // base unit and target unit.
            foreach (Ingredient i in Ingredients)
            {
                //we need to take the ingredient name, normalize it
                string normalizedDesc = i.Description;
                
                normalizedDesc = SearchAlgorithmHelper.NormalizePhraseSingular(normalizedDesc).Trim();
                
                //desc = SearchAlgorithmHelper.GetSoundExPhrase(desc);

                var fullUnit = SearchAlgorithmHelper.GetUnitFullName(unitTable, i.Unit);

                var abbreviatedUnit = SearchAlgorithmHelper.GetUnitAbbreviation(unitTable, i.Unit);

                //INSERT CODE HERE

                IngredientUnitList? subIngredient = new IngredientUnitList();

                //List<IngredientUnit> ingredientUnitTable =
                //    await LoadIngredientUnitTableAsync();

                var passes = new[]
                {
                    (Pass: 1, IsLiquid: true),
                    (Pass: 2, IsLiquid: true),
                    (Pass: 1, IsLiquid: false),
                    (Pass: 2, IsLiquid: false)
                };

                int passCount = 1;
                int? passFound = null;

                foreach (var passInfo in passes)
                {
                    subIngredient = RunPass(
                        ingredientUnitTable,
                        unitPrecisionRuleTable,
                        i.Description,
                        normalizedDesc,
                        fullUnit,
                        abbreviatedUnit,
                        i,
                        measurementSystem,
                        i.SortOrder,
                        i.IsActive,
                        passInfo.Pass,
                        passInfo.IsLiquid);

                    if (subIngredient != null)
                    {
                        subList.Add(subIngredient);
                        passFound = passCount;
                        break;
                    }
                    passCount++;
                }

                //var newIngredient = new Ingredient();
                //newIngredient.Description = i.Description;
                //newList.Add(newIngredient);

            }

            //Step 2.  Account for missing ingredients
            //Either system agnostic
            //or ingredient is not on the IngredientUnitList, so use standard conversion
            foreach (var ingredient in Ingredients)
            {
                // Normalize the original ingredient description to ensure an accurate match comparison
                string normalizedOriginalDesc = SearchAlgorithmHelper.NormalizePhraseSingular(ingredient.Description).Trim();

                // Check if this ingredient description does not exist anywhere in our processed newList
                //bool isFound = !newList.Any(n => string.Equals(n.Description, ingredient.Description, StringComparison.OrdinalIgnoreCase));
                bool isMissing = !subList.Any(n => string.Equals(n.Ingredient, ingredient.Description, StringComparison.OrdinalIgnoreCase));

                if (isMissing)
                {
                    //First handle cases where unit is system agnostic
                    if (unitTable.Any(u =>
                    (
                        string.Equals(u.Description, ingredient.Unit, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(u.Abbreviation, ingredient.Unit, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(u.Plural, ingredient.Unit, StringComparison.OrdinalIgnoreCase)
                    )
                    && u.System == null))
                    {
                        IngredientUnitList? subIngredient = new IngredientUnitList();
                        subIngredient.Ingredient = ingredient.Description;
                        subIngredient.NormalizedIngredient = normalizedOriginalDesc;
                        subIngredient.BaseUnit = ingredient.Unit;
                        subIngredient.TargetUnit = ingredient.Unit;
                        subIngredient.TargetUnitAlternative = null;
                        subIngredient.Quantity = ingredient.Quantity;
                        subIngredient.QuantityMax = ingredient.QuantityMax;
                        subIngredient.TargetSystem = measurementSystem == MeasurementSystem.Imperial ? MeasurementSystem.Metric : MeasurementSystem.Imperial;
                        subIngredient.Convert = false;
                        subIngredient.SortOrder = ingredient.SortOrder;
                        subIngredient.IsActive = ingredient.IsActive;
                        subList.Add(subIngredient);
                    }
                    //Do standard conversion
                    else
                    {
                        IngredientUnitList? subIngredient = new IngredientUnitList();
                        subIngredient.Ingredient = ingredient.Description;
                        subIngredient.NormalizedIngredient = normalizedOriginalDesc;
                        subIngredient.BaseUnit = ingredient.Unit;
                        //Get standard unit to convert
                        var singularUnit = unitTable
                            .FirstOrDefault(u =>
                                string.Equals(u.Description, ingredient.Unit, StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(u.Abbreviation, ingredient.Unit, StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(u.Plural, ingredient.Unit, StringComparison.OrdinalIgnoreCase))
                            ?.Description ?? ingredient.Unit;

                        subIngredient.TargetUnit = unitConversionTable
                            .FirstOrDefault(x =>
                                x.BaseSystem == (int)measurementSystem &&
                                string.Equals(x.BaseUnit, singularUnit, StringComparison.OrdinalIgnoreCase))
                            ?.TargetUnit;

                        //subIngredient.TargetUnit = unitConversionTable
                        //.FirstOrDefault(x =>
                        //    x.BaseSystem == (int)measurementSystem &&
                        //    string.Equals(x.BaseUnit, ingredient.Unit, StringComparison.OrdinalIgnoreCase))
                        //?.TargetUnit;
                        //subIngredient.TargetUnit = ingredient.Unit;
                        subIngredient.TargetUnitAlternative = null;

                        subIngredient.Quantity = ingredient.Quantity;
                        subIngredient.QuantityMax = ingredient.QuantityMax;
                        subIngredient.TargetSystem = measurementSystem == MeasurementSystem.Imperial ? MeasurementSystem.Metric : MeasurementSystem.Imperial;
                        subIngredient.Convert = true;
                        subIngredient.SortOrder = ingredient.SortOrder;
                        subIngredient.IsActive = ingredient.IsActive;
                        subList.Add(subIngredient);
                    }
                    var placeholder = 0;
                }


            }

            //Step 3.  Check Exception list (UnitConversionExceptions), and get proper conversion rate
            //          If not on exception list, get UnitConversion table
            foreach (var ingredient in subList)
            {
                var baseUnit = ingredient.BaseUnit;
                var targetUnit = ingredient.TargetUnit;

                string comparisonBaseUnit =
                    GetUnitSingular(ingredient.BaseUnit, unitTable);

                string comparisonTargetUnit =
                    GetUnitSingular(ingredient.TargetUnit, unitTable);

                string normalizedIngredient = ingredient.NormalizedIngredient;

                float conversionFactor = 1.0f;

                // Step 3a - Check UnitConversionExceptions first
                var exceptionEntry = unitConversionExceptionTable.FirstOrDefault(x =>
                !string.IsNullOrEmpty(x.Ingredient) && normalizedIngredient.Contains(x.Ingredient, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.BaseUnit, comparisonBaseUnit, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.TargetUnit, comparisonTargetUnit, StringComparison.OrdinalIgnoreCase) &&
                x.BaseSystem == (int)measurementSystem);


                if (exceptionEntry != null)
                {
                    ingredient.ConversionFactor = exceptionEntry.Quantity;
                }
                else
                {
                    // Step 3b - Fallback to UnitConversion table
                    var conversionEntry = unitConversionTable.FirstOrDefault(x =>
                        string.Equals(x.BaseUnit, comparisonBaseUnit, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(x.TargetUnit, comparisonTargetUnit, StringComparison.OrdinalIgnoreCase) &&
                        x.BaseSystem == (int)measurementSystem);

                    if (conversionEntry != null)
                    {
                        ingredient.ConversionFactor = conversionEntry.Quantity;
                    }
                    else
                    {
                        // If found nowhere, conversionFactor remains 1.0
                        ingredient.ConversionFactor = 1.0f;
                    }
                }

                
            }


            foreach (var ingredient in subList)
            {
                //Step 4.  Perform initial conversion
                if (ingredient.Convert)
                {
                    ingredient.TargetQuantity = ingredient.Quantity * ingredient.ConversionFactor;
                    ingredient.TargetQuantityMax = ingredient.QuantityMax * ingredient.ConversionFactor;
                }
                else
                {
                    ingredient.TargetQuantity = ingredient.Quantity;
                    ingredient.TargetQuantityMax = ingredient.QuantityMax;
                }

                //Step 5.  Get better unit
                //string canonicalUnit = GetUnitAbbreviationOrSingular(ingredient.TargetUnit, unitTable);

                //// Find all matching precision rules for the current canonical unit
                //var matchingRules = unitPrecisionRuleTable
                //    .Where(r => r.Unit == canonicalUnit)
                //    .OrderBy(r => r.Id) // Keeps processing sequential based on ID order
                //    .ToList();

                //foreach (var rule in matchingRules)
                //{
                //    // If Comparison or Threshold = null, skip everything else
                //    if (string.IsNullOrEmpty(rule.Comparison) || !rule.Threshold.HasValue)
                //    {
                //        continue;
                //    }

                //    bool evaluationResult = false;

                //    // Perform comparison dynamically based on the Comparison string operator
                //    if (rule.Comparison == ">=")
                //    {
                //        evaluationResult = ingredient.TargetQuantity >= rule.Threshold.Value;
                //    }
                //    else if (rule.Comparison == "<")
                //    {
                //        evaluationResult = ingredient.TargetQuantity < rule.Threshold.Value;
                //    }

                //    // If the comparison matches, update the unit and short-circuit out of the loop
                //    if (evaluationResult)
                //    {
                //        ingredient.TargetUnit = rule.NewUnit;
                //        ingredient.TargetQuantity = ingredient.TargetQuantity * rule.ConversionFactor;
                //        ingredient.TargetQuantityMax = ingredient.TargetQuantityMax * rule.ConversionFactor;
                //        break; // Stop and skip performing any remaining comparisons
                //    }
                //}
                // Step 5. Get better unit
                string canonicalUnit = GetUnitAbbreviationOrSingular(ingredient.TargetUnit, unitTable);

                if (ingredient.IngredientType == null)
                {
                    // Original logic for ingredients without a specified IngredientType
                    var matchingRules = unitPrecisionRuleTable
                        .Where(r => r.Unit == canonicalUnit)
                        .OrderBy(r => r.Id)
                        .ToList();

                    foreach (var rule in matchingRules)
                    {
                        // If Comparison or Threshold = null, skip everything else
                        if (string.IsNullOrEmpty(rule.Comparison) || !rule.Threshold.HasValue)
                        {
                            continue;
                        }

                        bool evaluationResult = false;

                        // Perform comparison dynamically based on the Comparison string operator
                        if (rule.Comparison == ">=")
                        {
                            evaluationResult = ingredient.TargetQuantity >= rule.Threshold.Value;
                        }
                        else if (rule.Comparison == "<")
                        {
                            evaluationResult = ingredient.TargetQuantity < rule.Threshold.Value;
                        }

                        // If the comparison matches, update the unit and short-circuit out of the loop
                        if (evaluationResult)
                        {
                            ingredient.TargetUnit = rule.NewUnit;
                            ingredient.TargetQuantity = ingredient.TargetQuantity * rule.ConversionFactor;
                            ingredient.TargetQuantityMax = ingredient.TargetQuantityMax * rule.ConversionFactor;
                            break;
                        }
                    }
                }
                else
                {
                    // New logic for ingredients with a specified IngredientType
                    var matchingRules = unitPrecisionRuleTable
                        .Where(r => r.IngredientType == ingredient.IngredientType)
                        .OrderBy(r => r.Id)
                        .ToList();

                    UnitPrecisionRule? selectedRule = null;

                    foreach (var rule in matchingRules)
                    {
                        // Only >= comparisons are used for IngredientType-specific rules
                        if (!rule.Threshold.HasValue || string.IsNullOrEmpty(rule.NewUnit))
                        {
                            continue;
                        }

                        if (ingredient.TargetQuantity >= rule.Threshold.Value)
                        {
                            // Keep updating so the last matching rule wins
                            selectedRule = rule;
                        }
                    }

                    // Apply the last matching rule, if one was found
                    if (selectedRule != null)
                    {
                        ingredient.TargetUnit = selectedRule.NewUnit;
                        ingredient.TargetQuantity *= selectedRule.ConversionFactor;
                        ingredient.TargetQuantityMax *= selectedRule.ConversionFactor;
                    }
                }

                //Step 6.  Round to user-friendly number, and set final values

                string canonicalBetterUnit = GetUnitAbbreviationOrSingular(ingredient.TargetUnit, unitTable);

                // Guard Clause: If the ingredient does not need conversion, keep original values and skip rounding
                if (!ingredient.Convert)
                {
                    ingredient.FinalQuantity = ingredient.Quantity?.ToString() ?? null;
                    ingredient.FinalQuantityMax = ingredient.QuantityMax?.ToString() ?? null;
                    continue; // Skip the metric/imperial rounding logic entirely
                }

                // Define or determine if the current target unit is metric
                // (You can swap this with your own metric tracking array/logic if needed)
                bool isMetric = ingredient.TargetSystem == MeasurementSystem.Metric;

                if (isMetric)
                {
                    // ---------------- METRIC ROUNDING LOGIC ----------------
                    // Find the rule for the NEW/CURRENT target unit to determine how to round it
                    var metricRule = unitPrecisionRuleTable.FirstOrDefault(r => r.Unit == canonicalBetterUnit);

                    if (metricRule != null)
                    {
                        // Process TargetQuantity
                        if (ingredient.TargetQuantity.HasValue)
                        {
                            float roundedQty;

                            // Exception case: if unit is g or ml and the quantity is less than 1, round to the nearest 0.05
                            if ((canonicalBetterUnit == "g" || canonicalBetterUnit == "ml") && ingredient.TargetQuantity.Value < 1f)
                            {
                                roundedQty = (float)Math.Round(ingredient.TargetQuantity.Value / 0.05f) * 0.05f;
                            }
                            else
                            {
                                float roundToValue;
                                if (!metricRule.Condition.HasValue)
                                {
                                    roundToValue = metricRule.RoundToElse ?? 1f;
                                }
                                else
                                {
                                    roundToValue = (ingredient.TargetQuantity.Value <= metricRule.Condition.Value)
                                        ? (metricRule.RoundToConditional ?? 1f)
                                        : (metricRule.RoundToElse ?? 1f);
                                }

                                // Round to the nearest chosen increment
                                roundedQty = (float)Math.Round(ingredient.TargetQuantity.Value / roundToValue) * roundToValue;
                            }

                            ingredient.FinalQuantity = roundedQty.ToString();
                        }

                        // Process TargetQuantityMax (Repeat all logic for QtyMax)
                        if (ingredient.TargetQuantityMax.HasValue)
                        {
                            float roundedQtyMax;

                            // Exception case: if unit is g or ml and the quantity max is less than 1, round to the nearest 0.05
                            if ((canonicalBetterUnit == "g" || canonicalBetterUnit == "ml") && ingredient.TargetQuantityMax.Value < 1f)
                            {
                                roundedQtyMax = (float)Math.Round(ingredient.TargetQuantityMax.Value / 0.05f) * 0.05f;
                            }
                            else
                            {
                                float roundToValueMax;
                                if (!metricRule.Condition.HasValue)
                                {
                                    roundToValueMax = metricRule.RoundToElse ?? 1f;
                                }
                                else
                                {
                                    // Evaluate criteria using TargetQuantityMax independently
                                    roundToValueMax = (ingredient.TargetQuantityMax.Value <= metricRule.Condition.Value)
                                        ? (metricRule.RoundToConditional ?? 1f)
                                        : (metricRule.RoundToElse ?? 1f);
                                }

                                // Round to the nearest chosen increment
                                roundedQtyMax = (float)Math.Round(ingredient.TargetQuantityMax.Value / roundToValueMax) * roundToValueMax;
                            }

                            ingredient.FinalQuantityMax = roundedQtyMax.ToString();
                        }
                    }
                }
                else
                {
                    // ---------------- IMPERIAL ROUNDING LOGIC ----------------
                    // Process TargetQuantity
                    if (ingredient.TargetQuantity.HasValue)
                    {
                        float totalValue = ingredient.TargetQuantity.Value;
                        int wholePart = (int)Math.Truncate(totalValue);
                        float decimalPart = totalValue - wholePart;

                        // Handle edge case if floating precision gives a tiny negative remainder
                        if (decimalPart < 0) decimalPart = 0;

                        // Find matching row in imperialRoundingRuleTable
                        var matchingFractionRule = imperialRoundingRuleTable.FirstOrDefault(r =>
                            decimalPart >= r.Lower && decimalPart < r.Upper);

                        if (matchingFractionRule != null)
                        {
                            if (matchingFractionRule.MapToFraction.Contains("/"))
                            {
                                // Format as a mixed fraction if whole part exists, otherwise just the fraction
                                ingredient.FinalQuantity = wholePart > 0
                                    ? $"{wholePart} {matchingFractionRule.MapToFraction}"
                                    : matchingFractionRule.MapToFraction;
                            }
                            else
                            {
                                // MapToFraction is a whole number string (e.g. "0" or "1")
                                if (int.TryParse(matchingFractionRule.MapToFraction, out int fractionAsInt))
                                {
                                    int finalWholeNumber = wholePart + fractionAsInt;
                                    ingredient.FinalQuantity = finalWholeNumber.ToString();
                                }
                                else
                                {
                                    ingredient.FinalQuantity = wholePart.ToString();
                                }
                            }
                        }
                        else
                        {
                            ingredient.FinalQuantity = totalValue.ToString();
                        }
                    }

                    // Process TargetQuantityMax (Repeat identical logic for QtyMax)
                    if (ingredient.TargetQuantityMax.HasValue)
                    {
                        float totalValueMax = ingredient.TargetQuantityMax.Value;
                        int wholePartMax = (int)Math.Truncate(totalValueMax);
                        float decimalPartMax = totalValueMax - wholePartMax;

                        if (decimalPartMax < 0) decimalPartMax = 0;

                        var matchingFractionRuleMax = imperialRoundingRuleTable.FirstOrDefault(r =>
                            decimalPartMax >= r.Lower && decimalPartMax < r.Upper);

                        if (matchingFractionRuleMax != null)
                        {
                            if (matchingFractionRuleMax.MapToFraction.Contains("/"))
                            {
                                ingredient.FinalQuantityMax = wholePartMax > 0
                                    ? $"{wholePartMax} {matchingFractionRuleMax.MapToFraction}"
                                    : matchingFractionRuleMax.MapToFraction;
                            }
                            else
                            {
                                if (int.TryParse(matchingFractionRuleMax.MapToFraction, out int fractionAsIntMax))
                                {
                                    int finalWholeNumberMax = wholePartMax + fractionAsIntMax;
                                    ingredient.FinalQuantityMax = finalWholeNumberMax.ToString();
                                }
                                else
                                {
                                    ingredient.FinalQuantityMax = wholePartMax.ToString();
                                }
                            }
                        }
                        else
                        {
                            ingredient.FinalQuantityMax = totalValueMax.ToString();
                        }
                    }
                }
                // --- END OF STEP 6 ---

            }

            FractionHelperDB fhdb = new FractionHelperDB(_context);
            List<FractionDecimal> fractionTable = await fhdb.LoadFractionTableAsync();
            var targetMeasurementSystem = measurementSystem == MeasurementSystem.Imperial ? MeasurementSystem.Metric : MeasurementSystem.Imperial;

            //Build new list of ingredients, with converted units
            foreach (var subIngredient in subList)
            {
                var ingredient = new Ingredient();
                ingredient.Description = subIngredient.Ingredient;

                ingredient.Quantity = subIngredient.FinalQuantity != null ? FractionHelper.ConvertStringToDbValueBySystem(subIngredient.FinalQuantity, fractionTable, targetMeasurementSystem) : null;
                ingredient.QuantityMax = subIngredient.FinalQuantityMax != null ? FractionHelper.ConvertStringToDbValueBySystem(subIngredient.FinalQuantityMax, fractionTable, targetMeasurementSystem) : null;
                //ingredient.Quantity = subIngredient.FinalQuantity != null ?  Convert.ToInt32(subIngredient.FinalQuantity) : null;
                //ingredient.QuantityMax = subIngredient.FinalQuantityMax != null ? Convert.ToInt32(subIngredient.FinalQuantityMax) : null;
                //ingredient.Quantity = subIngredient.Quantity;
                //ingredient.QuantityMax = subIngredient.QuantityMax;
                ingredient.Unit = subIngredient.TargetUnit;
                ingredient.SortOrder = subIngredient.SortOrder;
                ingredient.IsActive = subIngredient.IsActive;
                

                newList.Add(ingredient);
            }


            return newList;
            //return subList;
        }



        //private IngredientUnitList? RunPass(
        //    List<IngredientUnit> ingredientUnitTable,
        //    string desc,
        //    string normalizedDesc,
        //    string fullUnit,
        //    string abbreviatedUnit,
        //    Ingredient ingredient,
        //    MeasurementSystem measurementSystem,
        //    int? sortOrder,
        //    bool isActive,
        //    int pass,
        //    bool isLiquid)
        //{
        //    if (measurementSystem == MeasurementSystem.Imperial)
        //    {
        //        var entry = ingredientUnitTable.FirstOrDefault(x =>
        //            x.Pass == pass &&
        //            x.IsLiquid == isLiquid &&
        //            desc.Contains(x.Ingredient, StringComparison.OrdinalIgnoreCase) &&
        //            (
        //                string.Equals(abbreviatedUnit, x.LargeImperialUnit, StringComparison.OrdinalIgnoreCase) ||
        //                string.Equals(abbreviatedUnit, x.SmallImperialUnit, StringComparison.OrdinalIgnoreCase)
        //            ));

        //        if (entry != null)
        //        {
        //            return new IngredientUnitList
        //            {
        //                Ingredient = desc,
        //                NormalizedIngredient = normalizedDesc,
        //                BaseUnit = fullUnit,
        //                TargetUnit = entry.MetricUnit,
        //                TargetUnitAlternative = null,
        //                Quantity = ingredient.Quantity,
        //                QuantityMax = ingredient.QuantityMax,
        //                TargetSystem = MeasurementSystem.Metric,
        //                Convert = true,
        //                SortOrder = sortOrder,
        //                IsActive = isActive
        //            };
        //        }
        //    }
        //    else if (measurementSystem == MeasurementSystem.Metric)
        //    {
        //        var entry = ingredientUnitTable.FirstOrDefault(x =>
        //            x.Pass == pass &&
        //            x.IsLiquid == isLiquid &&
        //            desc.Contains(x.Ingredient, StringComparison.OrdinalIgnoreCase) &&
        //            string.Equals(abbreviatedUnit, x.MetricUnit, StringComparison.OrdinalIgnoreCase));

        //        if (entry != null)
        //        {
        //            return new IngredientUnitList
        //            {
        //                Ingredient = desc,
        //                NormalizedIngredient = normalizedDesc,
        //                BaseUnit = fullUnit,
        //                TargetUnit = entry.SmallImperialUnit,
        //                TargetUnitAlternative = null,
        //                Quantity = ingredient.Quantity,
        //                QuantityMax = ingredient.QuantityMax,
        //                TargetSystem = MeasurementSystem.Imperial,
        //                Convert = true,
        //                SortOrder = sortOrder,
        //                IsActive = isActive
        //            };
        //        }
        //    }

        //    return null;
        //}


        //private IngredientUnitList? RunPass(
        //    List<IngredientUnit> ingredientUnitTable,
        //    List<UnitPrecisionRule> unitPrecisionRuleTable,
        //    string desc,
        //    string normalizedDesc,
        //    string fullUnit,
        //    string abbreviatedUnit,
        //    Ingredient ingredient,
        //    MeasurementSystem measurementSystem,
        //    int? sortOrder,
        //    bool isActive,
        //    int pass,
        //    bool isLiquid)
        //{
        //    // Condition 1 - Ingredient match
        //    var ingredientEntry = ingredientUnitTable.FirstOrDefault(x =>
        //        x.Pass == pass &&
        //        x.IsLiquid == isLiquid &&
        //        desc.Contains(x.Ingredient, StringComparison.OrdinalIgnoreCase));

        //    if (ingredientEntry == null)
        //        return null;

        //    // Get IngredientType from IngredientUnit table
        //    var ingredientType = ingredientEntry.IngredientType;

        //    // Condition 2 - UnitPrecisionRule match
        //    var precisionRule = unitPrecisionRuleTable.FirstOrDefault(x =>
        //        x.IngredientType == ingredientType &&
        //        (
        //            string.Equals(x.Unit, abbreviatedUnit, StringComparison.OrdinalIgnoreCase) ||
        //            string.Equals(x.NewUnit, abbreviatedUnit, StringComparison.OrdinalIgnoreCase)
        //        ));

        //    if (precisionRule == null)
        //        return null;

        //    if (measurementSystem == MeasurementSystem.Imperial)
        //    {
        //        return new IngredientUnitList
        //        {
        //            Ingredient = desc,
        //            NormalizedIngredient = normalizedDesc,
        //            BaseUnit = fullUnit,
        //            TargetUnit = ingredientEntry.MetricUnit,
        //            TargetUnitAlternative = null,
        //            Quantity = ingredient.Quantity,
        //            QuantityMax = ingredient.QuantityMax,
        //            TargetSystem = MeasurementSystem.Metric,
        //            Convert = true,
        //            SortOrder = sortOrder,
        //            IsActive = isActive,
        //            IngredientType = ingredientType != null ? ingredientType : null
        //        };
        //    }
        //    else if (measurementSystem == MeasurementSystem.Metric)
        //    {
        //        return new IngredientUnitList
        //        {
        //            Ingredient = desc,
        //            NormalizedIngredient = normalizedDesc,
        //            BaseUnit = fullUnit,
        //            TargetUnit = ingredientEntry.SmallImperialUnit,
        //            TargetUnitAlternative = null,
        //            Quantity = ingredient.Quantity,
        //            QuantityMax = ingredient.QuantityMax,
        //            TargetSystem = MeasurementSystem.Imperial,
        //            Convert = true,
        //            SortOrder = sortOrder,
        //            IsActive = isActive,
        //            IngredientType = ingredientType != null ? ingredientType : null
        //        };
        //    }

        //    return null;
        //}

        private IngredientUnitList? RunPass(
        List<IngredientUnit> ingredientUnitTable,
        List<UnitPrecisionRule> unitPrecisionRuleTable,
        string desc,
        string normalizedDesc,
        string fullUnit,
        string abbreviatedUnit,
        Ingredient ingredient,
        MeasurementSystem measurementSystem,
        int? sortOrder,
        bool isActive,
        int pass,
        bool isLiquid)
        {
            // Condition 1 - Ingredient match
            var ingredientEntry = ingredientUnitTable.FirstOrDefault(x =>
                x.Pass == pass &&
                x.IsLiquid == isLiquid &&
                desc.Contains(x.Ingredient, StringComparison.OrdinalIgnoreCase));

            if (ingredientEntry == null)
                return null;

            // Get IngredientType from IngredientUnit table
            var ingredientType = ingredientEntry.IngredientType;

            // Condition 2 - Match based on measurement system
            if (measurementSystem == MeasurementSystem.Imperial)
            {
                // Existing logic for Metric
                var precisionRule = unitPrecisionRuleTable.FirstOrDefault(x =>
                    x.IngredientType == ingredientType &&
                    (
                        string.Equals(x.Unit, abbreviatedUnit, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(x.NewUnit, abbreviatedUnit, StringComparison.OrdinalIgnoreCase)
                    ));

                if (precisionRule == null)
                    return null;
            }
            else if (measurementSystem == MeasurementSystem.Metric)
            {
                // New logic for Imperial: match MetricUnit or LargeMetricUnit
                var metricUnitMatch = ingredientUnitTable.FirstOrDefault(x =>
                    x.IngredientType == ingredientType &&
                    (
                        string.Equals(x.MetricUnit, abbreviatedUnit, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(x.LargeMetricUnit, abbreviatedUnit, StringComparison.OrdinalIgnoreCase)
                    ));

                if (metricUnitMatch == null)
                    return null;
            }
            else
            {
                return null;
            }

            // Return conversion result
            if (measurementSystem == MeasurementSystem.Imperial)
            {
                return new IngredientUnitList
                {
                    Ingredient = desc,
                    NormalizedIngredient = normalizedDesc,
                    BaseUnit = fullUnit,
                    TargetUnit = ingredientEntry.MetricUnit,
                    TargetUnitAlternative = null,
                    Quantity = ingredient.Quantity,
                    QuantityMax = ingredient.QuantityMax,
                    TargetSystem = MeasurementSystem.Metric,
                    Convert = true,
                    SortOrder = sortOrder,
                    IsActive = isActive,
                    IngredientType = ingredientType
                };
            }
            else if (measurementSystem == MeasurementSystem.Metric)
            {
                return new IngredientUnitList
                {
                    Ingredient = desc,
                    NormalizedIngredient = normalizedDesc,
                    BaseUnit = fullUnit,
                    TargetUnit = ingredientEntry.SmallImperialUnit,
                    TargetUnitAlternative = null,
                    Quantity = ingredient.Quantity,
                    QuantityMax = ingredient.QuantityMax,
                    TargetSystem = MeasurementSystem.Imperial,
                    Convert = true,
                    SortOrder = sortOrder,
                    IsActive = isActive,
                    IngredientType = ingredientType
                };
            }

            return null;
        }

        #region Helper Functions
        public static string GetUnitSingular(string unit, List<Unit> unitTable)
        {
            if (string.IsNullOrWhiteSpace(unit))
                return unit;

            var match = unitTable.FirstOrDefault(u =>
                string.Equals(u.Description, unit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Plural, unit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Abbreviation, unit, StringComparison.OrdinalIgnoreCase));

            return match?.Description ?? unit;
        }

        public static string GetUnitAbbreviationOrSingular(string unit, List<Unit> unitTable)
        {
            if (string.IsNullOrWhiteSpace(unit))
                return unit;

            var match = unitTable.FirstOrDefault(u =>
                string.Equals(u.Description, unit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Plural, unit, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Abbreviation, unit, StringComparison.OrdinalIgnoreCase));

            if (match == null)
                return unit;

            return !string.IsNullOrWhiteSpace(match.Abbreviation)
                ? match.Abbreviation
                : match.Description ?? unit;
        }

        #endregion
    }
}
