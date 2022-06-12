using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace TeleDoc.API.Services.EmailServices;

public class MailJetEmailSender : IEmailSender
{
    private readonly MailJetOptions _mailJetOptions;

    public MailJetEmailSender(IConfiguration configuration)
    {
        _mailJetOptions = configuration.GetSection("MailJet").Get<MailJetOptions>();
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        MailjetClient client = new MailjetClient(_mailJetOptions.ApiKey, _mailJetOptions.SecretKey) {
            Version = ApiVersion.V3_1,
        };
        MailjetRequest request = new MailjetRequest {
                Resource = Send.Resource,
            }
            .Property(Send.Messages, new JArray {
                new JObject {
                    {
                        "From",
                        new JObject {
                            {"Email", "gamingsvo22@protonmail.com"}, 
                            {"Name", "TeleDoc"}
                        }
                    }, {
                        "To",
                        new JArray {
                            new JObject {
                                {
                                    "Email",
                                    email
                                }, {
                                    "Name",
                                    "TeleDoc"
                                }
                            }
                        }
                    }, {
                        "Subject",
                        subject
                    }, {
                        "TextPart",
                        "My first Mailjet email"
                    }, {
                        "HTMLPart",
                        htmlMessage
                    }
                }
            });
        await client.PostAsync(request);
        // MailjetResponse response = await client.PostAsync(request);
        // if (response.IsSuccessStatusCode) {
        //     Console.WriteLine($"Total: {response.GetTotal()}, Count: {response.GetCount()}\n");
        //     Console.WriteLine(response.GetData());
        // } else {
        //     Console.WriteLine($"StatusCode: {response.StatusCode}\n");
        //     Console.WriteLine($"ErrorInfo: {response.GetErrorInfo()}\n");
        //     Console.WriteLine(response.GetData());
        //     Console.WriteLine($"ErrorMessage: {response.GetErrorMessage()}\n");
        // }
    }
}
