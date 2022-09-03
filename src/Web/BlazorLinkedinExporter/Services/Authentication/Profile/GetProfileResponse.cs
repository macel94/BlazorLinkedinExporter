namespace BlazorLinkedinExporter.Services.Authentication.Profile
{
    public class GetProfileResponse
    {
        public FirstnameObj? FirstName { get; set; }
        public LastnameObj? LastName { get; set; }
        public ProfilepictureObj? ProfilePicture { get; set; }
        public string? Id { get; set; }
    }
}
