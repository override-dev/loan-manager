using System.ComponentModel.DataAnnotations;

namespace Loan.Shared.Contracts.Models;

public record MessageEnvelope([Required(AllowEmptyStrings = false)] string MessageType, [Required(AllowEmptyStrings = false)] string MessageContent);
