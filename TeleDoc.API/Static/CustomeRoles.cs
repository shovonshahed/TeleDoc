namespace TeleDoc.API.Static;

public class CustomeRoles
{
    public const string PatientAdmin  = UserRoles.Admin + "," + UserRoles.Patient;
    public const string DoctorAdmin = UserRoles.Admin + "," + UserRoles.Doctor;
    public const string PatientDoctor = UserRoles.Patient + "," + UserRoles.Doctor;


}