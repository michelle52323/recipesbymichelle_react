namespace PlatformAPI.Enums
{

    //This enum classifies ingredients by how they are measured.
    //In most cases, this lines up with their technical classification
    //However, when there is a conflict between technical classification and 
    //  measurement classification, we go by how an ingredient is measured
    //Example: Flour is technically a flexible, but for measurement purposes, is
    //  classified as a flexible, because it can be measured in oz (pow
    public enum IngredientType
    {
        Solid = 1,
        Liquid = 2,
        Powder = 3,         //spices, salt, and other powders
        OilSyrup = 4,       //oils and syrups are classified as oil syrup
        Flexible = 5,       //rice, oats, butter
    }
}