namespace BlazorLinkedinExporter.Services.Authentication.Profile
{
    public class Element
    {
        public string? Artifact { get; set; }
        public string? AuthorizationMethod { get; set; }
        public Data? Data { get; set; }
        public IdentifierObj[]? Identifiers { get; set; }
    }

}
