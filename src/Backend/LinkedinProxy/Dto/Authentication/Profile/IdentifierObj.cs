namespace LinkedinProxy.Dto.Authentication.Profile
{
    public class IdentifierObj
    {
        public string? Identifier { get; set; }
        public int Index { get; set; }
        public string? MediaType { get; set; }
        public string? File { get; set; }
        public string? IdentifierType { get; set; }
        public int IdentifierExpiresInSeconds { get; set; }
    }

}
