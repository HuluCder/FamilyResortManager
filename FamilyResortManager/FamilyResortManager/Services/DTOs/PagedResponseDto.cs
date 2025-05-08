namespace FamilyResortManager.Services.DTOs;

public class PagedResponseDto<T>
{
    /// <summary>Элементы текущей страницы</summary>
    public IEnumerable<T> Items { get; set; } = new List<T>();
    /// <summary>Общее число элементов во всей выборке</summary>
    public int TotalCount { get; set; }
    /// <summary>Номер текущей страницы</summary>
    public int PageNumber { get; set; }
    /// <summary>Размер страницы</summary>
    public int PageSize { get; set; }
}