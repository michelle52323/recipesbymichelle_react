
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

        public bool Convert {  get; set; }
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

            //Step 1.  Traverse IngredientUnits table.  Find all ingredients and map to 
            // base unit and target unit.
            foreach (Ingredient i in Ingredients)
            {
                //we need to take the ingredient name, normalize it
                string normalizedDesc = i.Description;
                
                normalizedDesc = SearchAlgorithmHelper.NormalizePhraseSingular(normalizedDesc);
                
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
                        i.Description,
                        normalizedDesc,
                        fullUnit,
                        abbreviatedUnit,
                        i,
                        measurementSystem,
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

                var newIngredient = new Ingredient();
                newIngredient.Description = i.Description;
                newList.Add(newIngredient);

            }

            //Step 2.  Account for missing ingredients
            //Either system agnostic
            //or ingredient is not on the IngredientUnitList, so use standard conversion
            foreach (var ingredient in Ingredients)
            {
                // Normalize the original ingredient description to ensure an accurate match comparison
                string normalizedOriginalDesc = SearchAlgorithmHelper.NormalizePhraseSingular(ingredient.Description);

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
            return newList;
        }



        private IngredientUnitList? RunPass(
            List<IngredientUnit> ingredientUnitTable,
            string desc,
            string normalizedDesc,
            string fullUnit,
            string abbreviatedUnit,
            Ingredient ingredient,
            MeasurementSystem measurementSystem,
            int pass,
            bool isLiquid)
        {
            if (measurementSystem == MeasurementSystem.Imperial)
            {
                var entry = ingredientUnitTable.FirstOrDefault(x =>
                    x.Pass == pass &&
                    x.IsLiquid == isLiquid &&
                    desc.Contains(x.Ingredient, StringComparison.OrdinalIgnoreCase) &&
                    (
                        string.Equals(abbreviatedUnit, x.LargeImperialUnit, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(abbreviatedUnit, x.SmallImperialUnit, StringComparison.OrdinalIgnoreCase)
                    ));

                if (entry != null)
                {
                    return new IngredientUnitList
                    {
                        Ingredient = desc,
                        NormalizedIngredient = normalizedDesc,
                        BaseUnit = fullUnit,
                        TargetUnit = entry.MetricUnit,
                        TargetUnitAlternative = null,
                        Quantity = ingredient.Quantity,
                        QuantityMax = ingredient.QuantityMax,
                        TargetSystem = MeasurementSystem.Metric,
                        Convert = true,
                    };
                }
            }
            else if (measurementSystem == MeasurementSystem.Metric)
            {
                var entry = ingredientUnitTable.FirstOrDefault(x =>
                    x.Pass == pass &&
                    x.IsLiquid == isLiquid &&
                    desc.Contains(x.Ingredient, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(abbreviatedUnit, x.MetricUnit, StringComparison.OrdinalIgnoreCase));

                if (entry != null)
                {
                    return new IngredientUnitList
                    {
                        Ingredient = desc,
                        BaseUnit = fullUnit,
                        TargetUnit = entry.LargeImperialUnit,
                        TargetUnitAlternative = entry.SmallImperialUnit,
                        Quantity = ingredient.Quantity,
                        QuantityMax = ingredient.QuantityMax,
                        TargetSystem = MeasurementSystem.Imperial
                    };
                }
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
