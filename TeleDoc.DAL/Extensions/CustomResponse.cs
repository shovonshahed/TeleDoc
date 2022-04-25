using TeleDoc.DAL.Entities;
using TeleDoc.DAL.Enums;

namespace TeleDoc.DAL.Extensions;

public class CustomResponse
{
    public ResponseStatus Status;
    public ApplicationUser? User;
}