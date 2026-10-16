
using PlatformAPI.Models.Recipe;
using PlatformAPI.Models.UnitConversion;
using PlatformAPI.Enums;
using PlatformAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace PlatformAPI.Helpers
{

    public class IngredientUnitList
    {
        public string Ingredient { get; set; }

        public string BaseUnit { get; set; }

        public string TargetUnit { get; set; }

        public string TargetUnitAlternative { get; set; }

        public float? Quantity {  get; set; }

        public float? QuantityMax { get; set; }

        public MeasurementSystem TargetSystem { get; set; }
    }
    public class UnitConversion
    {

        private readonly DbContext _context;

        public UnitConversion(DbContext context)
        {
            _context = context;
        }

        public async Task<List<IngredientUnit>> LoadIngredientUnitTableAsync()
        {
            return await _context.Set<IngredientUnit>().ToListAsync();
        }

        public async Task<List<Ingredient>> GetConvertedUnit(List<Ingredient> Ingredients, MeasurementSystem measurementSystem)
        {
            List<IngredientUnitList> subList = new List<IngredientUnitList>();
            List<Ingredient> newList = new List<Ingredient>();

            var unitDB = new UnitDB(_context);
            List<Unit> unitTable = await unitDB.LoadUnitTableAsync();

            foreach (Ingredient i in Ingredients)
            {
                //we need to take the ingredient name, normalize it
                var desc = i.Description;
                desc = SearchAlgorithmHelper.NormalizeWordSingular(desc);
                
                //desc = SearchAlgorithmHelper.GetSoundExPhrase(desc);

                var fullUnit = SearchAlgorithmHelper.GetUnitFullName(unitTable, i.Unit);

                var abbreviatedUnit = SearchAlgorithmHelper.GetUnitAbbreviation(unitTable, i.Unit);

                //INSERT CODE HERE
                IngredientUnitList? subIngredient = null;

                List<IngredientUnit> ingredientUnitTable = await LoadIngredientUnitTableAsync(); ;

                // PASS 1
                if (measurementSystem == MeasurementSystem.Imperial)
                {
                    var entryA = ingredientUnitTable.FirstOrDefault(x =>
                        x.Pass == 1 &&
                        desc.Contains(x.Ingredient, StringComparison.OrdinalIgnoreCase) &&
                        (
                            string.Equals(abbreviatedUnit, x.LargeImperialUnit, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(abbreviatedUnit, x.SmallImperialUnit, StringComparison.OrdinalIgnoreCase)
                        ));

                    if (entryA != null)
                    {
                        subIngredient = new IngredientUnitList
                        {
                            Ingredient = desc,
                            BaseUnit = fullUnit,
                            TargetUnit = entryA.MetricUnit,
                            TargetUnitAlternative = null,
                            Quantity = i.Quantity,
                            QuantityMax = i.QuantityMax,
                            TargetSystem = MeasurementSystem.Metric
                        };
                    }
                }
                else if (measurementSystem == MeasurementSystem.Metric)
                {
                    var entryB = ingredientUnitTable.FirstOrDefault(x =>
                        x.Pass == 1 &&
                        desc.Contains(x.Ingredient, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(abbreviatedUnit, x.MetricUnit, StringComparison.OrdinalIgnoreCase));

                    if (entryB != null)
                    {
                        subIngredient = new IngredientUnitList
                        {
                            Ingredient = desc,
                            BaseUnit = fullUnit,
                            TargetUnit = entryB.LargeImperialUnit,
                            TargetUnitAlternative = entryB.SmallImperialUnit,
                            Quantity = i.Quantity,
                            QuantityMax = i.QuantityMax,
                            TargetSystem = MeasurementSystem.Imperial
                        };
                    }
                }

                // If found in pass 1, skip pass 2
                if (subIngredient != null)
                {
                    subList.Add(subIngredient);
                }
                else
                {
                    // PASS 2
                    if (measurementSystem == MeasurementSystem.Imperial)
                    {
                        var entryA = ingredientUnitTable.FirstOrDefault(x =>
                            x.Pass == 2 &&
                            desc.Contains(x.Ingredient, StringComparison.OrdinalIgnoreCase) &&
                            (
                                string.Equals(abbreviatedUnit, x.LargeImperialUnit, StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(abbreviatedUnit, x.SmallImperialUnit, StringComparison.OrdinalIgnoreCase)
                            ));

                        if (entryA != null)
                        {
                            subIngredient = new IngredientUnitList
                            {
                                Ingredient = desc,
                                BaseUnit = fullUnit,
                                TargetUnit = entryA.MetricUnit,
                                TargetUnitAlternative = null,
                                Quantity = i.Quantity,
                                QuantityMax = i.QuantityMax,
                                TargetSystem = MeasurementSystem.Metric
                            };
                        }
                    }
                    else if (measurementSystem == MeasurementSystem.Metric)
                    {
                        var entryB = ingredientUnitTable.FirstOrDefault(x =>
                            x.Pass == 2 &&
                            desc.Contains(x.Ingredient, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(abbreviatedUnit, x.MetricUnit, StringComparison.OrdinalIgnoreCase));

                        if (entryB != null)
                        {
                            subIngredient = new IngredientUnitList
                            {
                                Ingredient = desc,
                                BaseUnit = fullUnit,
                                TargetUnit = entryB.LargeImperialUnit,
                                TargetUnitAlternative = entryB.SmallImperialUnit,
                                Quantity = i.Quantity,
                                QuantityMax = i.QuantityMax,
                                TargetSystem = MeasurementSystem.Imperial
                            };
                        }
                    }

                    if (subIngredient != null)
                    {
                        subList.Add(subIngredient);
                    }
                }

                var newIngredient = new Ingredient();
                newIngredient.Description = desc;
                newList.Add(newIngredient);

            }




            return newList;
        }
    }
}
