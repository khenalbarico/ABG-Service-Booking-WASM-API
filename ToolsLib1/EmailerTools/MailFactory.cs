using CommonLib1.Models.Client;
using System.Net;
using System.Text;
using static CommonLib1.Models.Constants;

namespace ToolsLib1.EmailerTools;

public static class MailFactory
{
    public static string ComposeMailBody(
           this string   body,
           ClientRequest req)
    {
        var client   = req.ClientInformation;
        var services = req.ClientServices ?? [];

        var fullName  = $"{client?.FirstName} {client?.LastName}".Trim();
        var totalCost = services.Sum(x => x.ServiceCost);

        var bookingId   = client?.ClientBookingId ?? string.Empty;
        var bookingDate = client?.BookingDate ?? DateTime.MinValue;

        var servicesHtml = new StringBuilder();

        foreach (var service in services.OrderBy(x => x.ServiceDate))
        {
            servicesHtml.Append($@"
                <tr>
                    <td style='padding:18px; border-bottom:1px solid #d9d9d9;'>
                        <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0'>
                            <tr>
                                <td style='vertical-align:top; padding-right:16px;'>
                                    <div style='font-size:16px; font-weight:bold; color:#000000; margin-bottom:6px;'>
                                        {Encode(service.ServiceName)}
                                    </div>
                                    <div style='font-size:14px; color:#444444; margin-bottom:10px; line-height:1.6;'>
                                        {Encode(service.ServiceDetails)}
                                    </div>
                                </td>
                                <td align='right' style='vertical-align:top; white-space:nowrap;'>
                                    <div style='display:inline-block; padding:8px 14px; border:1px solid #000000; font-size:14px; font-weight:bold; color:#000000; background-color:#ffffff;'>
                                        ₱{service.ServiceCost:N2}
                                    </div>
                                </td>
                            </tr>
                        </table>

                        <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='margin-top:10px;'>
                            <tr>
                                <td width='50%' style='font-size:13px; color:#555555; padding:4px 12px 4px 0; vertical-align:top;'>
                                    <strong>Reference:</strong> {Encode(service.ServiceUid)}
                                </td>
                                <td width='50%' style='font-size:13px; color:#555555; padding:4px 0 4px 12px; vertical-align:top;'>
                                    <strong>Branch:</strong> {Encode(GetBranchDisplay(service.Branch))}
                                </td>
                            </tr>
                            <tr>
                                <td width='50%' style='font-size:13px; color:#555555; padding:4px 12px 4px 0; vertical-align:top;'>
                                    <strong>Design:</strong> {Encode(string.IsNullOrWhiteSpace(service.ServiceDesign) ? "N/A" : service.ServiceDesign)}
                                </td>
                                <td width='50%' style='font-size:13px; color:#555555; padding:4px 0 4px 12px; vertical-align:top;'>
                                    <strong>Date:</strong> {service.ServiceDate:MMMM d, yyyy}
                                </td>
                            </tr>
                            <tr>
                                <td width='50%' style='font-size:13px; color:#555555; padding:4px 12px 4px 0; vertical-align:top;'>
                                    <strong>Time:</strong> {service.ServiceDate:h:mm tt}
                                </td>
                                <td width='50%' style='font-size:13px; color:#555555; padding:4px 0 4px 12px; vertical-align:top;'>
                                    <strong>Status:</strong> Confirmed
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>");
        }

        return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>ABG Studios Service</title>
</head>
<body style='margin:0; padding:0; background-color:#f3f3f3; font-family:Arial, Helvetica, sans-serif; color:#000000;'>
    <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='background-color:#f3f3f3; margin:0; padding:24px 0;'>
        <tr>
            <td align='center'>
                <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='max-width:720px; background-color:#ffffff; border:1px solid #000000;'>
                    
                    <tr>
                        <td style='background-color:#000000; padding:28px 32px; text-align:center;'>
                            <div style='font-size:28px; font-weight:bold; color:#ffffff; letter-spacing:1px;'>
                                ABG STUDIOS
                            </div>
                            <div style='font-size:13px; color:#d9d9d9; margin-top:8px;'>
                                Service Booking Confirmation
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:32px;'>

                            <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='margin-bottom:24px; border:1px solid #000000;'>
                                <tr>
                                    <td style='background-color:#000000; color:#ffffff; padding:14px 18px; font-size:14px; text-align:left;'>
                                        <strong>Booking ID:</strong> {Encode(bookingId)}
                                    </td>
                                </tr>
                            </table>

                            <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='margin-bottom:28px; border:1px dashed #000000;'>
                                <tr>
                                    <td style='padding:22px 24px;'>
                                        <div style='font-size:12px; font-weight:bold; color:#555555; letter-spacing:1px; text-transform:uppercase; margin-bottom:8px;'>
                                            Booking Details
                                        </div>
                                        <div style='font-size:24px; font-weight:bold; color:#000000; margin-bottom:10px;'>
                                            {Encode(fullName)}
                                        </div>
                                        <div style='font-size:14px; line-height:1.8; color:#222222;'>
                                            <strong>Booking ID:</strong> {Encode(bookingId)}<br/>
                                            <strong>Booking Date:</strong> {BookingDateDisplay(bookingDate)}<br/>
                                            <strong>Total Services:</strong> {services.Count}<br/>
                                            <strong>Total Amount:</strong> ₱{totalCost:N2}
                                        </div>
                                    </td>
                                </tr>
                            </table>

                            <div style='font-size:22px; font-weight:bold; color:#000000; margin-bottom:16px;'>
                                Hello {Encode(fullName)},
                            </div>

                            <div style='font-size:15px; line-height:1.7; color:#222222; margin-bottom:24px;'>
                                Thank you for booking with <strong>ABG Studios</strong>.
                                Please present this email at the counter as your booking reference.
                            </div>

                            <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='border:1px solid #d9d9d9; margin-bottom:28px;'>
                                <tr>
                                    <td style='background-color:#fafafa; padding:16px 20px; border-bottom:1px solid #d9d9d9; font-size:16px; font-weight:bold; color:#000000;'>
                                        Booked Services
                                    </td>
                                </tr>
                                {servicesHtml}
                            </table>

                            <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='margin-bottom:28px;'>
                                <tr>
                                    <td style='background-color:#000000; color:#ffffff; padding:18px 20px; font-size:18px; font-weight:bold; text-align:right;'>
                                        Total Amount: ₱{totalCost:N2}
                                    </td>
                                </tr>
                            </table>

                            <div style='font-size:14px; line-height:1.7; color:#333333;'>
                                Regards,<br/>
                                <strong>ABG Studios</strong>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td style='background-color:#000000; padding:18px 24px; text-align:center;'>
                            <div style='font-size:12px; color:#ffffff;'>
                                © {DateTime.UtcNow.Year} ABG Studios. All rights reserved.
                            </div>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
    }

    public static string DefaultSubject(this string subject)
        => "ABG Studios Service";

    private static string Encode(string? value)
        => WebUtility.HtmlEncode(value ?? string.Empty);

    private static string BookingDateDisplay(DateTime value)
        => value == DateTime.MinValue
            ? "N/A"
            : $"{value:MMMM d, yyyy}, {value:dddd}, {value:h:mm tt}";

    private static string GetBranchDisplay(ServiceBranch branch)
        => BranchNames.TryGetValue(branch, out var branchName)
            ? branchName
            : branch.ToString();
}