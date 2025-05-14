using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FinAssist.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    [Display(Name = "Доход")]
    Income = 1,
    [Display(Name = "Расход")]
    Expense = 2,
}