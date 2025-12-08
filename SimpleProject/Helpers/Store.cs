namespace SimpleProject.Helpers
{
    public static class Store
    {

        public static List<Value> Chooses { get; set; } = new List<Value>()
        {
           new Value(){ KeyValue=false,NameAr="لا",NameEn="No"},
            new Value(){ KeyValue=true,NameAr="نعم",NameEn="yes"}
        };
    }
}
