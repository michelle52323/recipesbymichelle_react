
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

                List<IngredientUnit> ingredientUnitTable =
                    await LoadIngredientUnitTableAsync();

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
                        desc,
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
                newIngredient.Description = desc;
                newList.Add(newIngredient);

            }




            return newList;
        }



        private IngredientUnitList? RunPass(
            List<IngredientUnit> ingredientUnitTable,
            string desc,
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
                        BaseUnit = fullUnit,
                        TargetUnit = entry.MetricUnit,
                        TargetUnitAlternative = null,
                        Quantity = ingredient.Quantity,
                        QuantityMax = ingredient.QuantityMax,
                        TargetSystem = MeasurementSystem.Metric
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
    }
}
