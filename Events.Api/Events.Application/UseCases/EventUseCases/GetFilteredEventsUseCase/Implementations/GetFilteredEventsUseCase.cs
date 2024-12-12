using Events.Application.Models.Event;
using Events.Application.UseCases.EventUseCases.GetEventsWithPaginationUseCase;
using Events.Application.Validators.Event;

namespace Events.Application.UseCases.EventUseCases.GetFilteredEventsUseCase.Implementations;

internal class GetFilteredEventsUseCase : IGetFilteredEventsUseCase
{
    private const string PLACEFILTER = "place";
    private const string MAXMEMBERSFILTER = "maxmembers";
    private const string CATEGORYIDFILTER = "categoryid";
    private const string CREATORIDFILTER = "creatorid";
    private readonly IGetEventsWithPaginationUseCase getEventsWithPaginationUseCase;
    private readonly PageValidator pageValidator;
    private readonly FilterItemValidator filterItemValidator;

    public GetFilteredEventsUseCase(IGetEventsWithPaginationUseCase getEventsWithPaginationUseCase,
        PageValidator pageValidator,
        FilterItemValidator filterItemValidator)
    {
        this.getEventsWithPaginationUseCase = getEventsWithPaginationUseCase;
        this.pageValidator = pageValidator;
        this.filterItemValidator = filterItemValidator;
    }

    public IEnumerable<GetEventsResponseDTO> Execute(int page, string filterItem, string filterValue)
    {
        if (!pageValidator.Validate(page).IsValid)
        {
            throw new InvalidDataException("Page must be 1 or greater");
        }

        if (!filterItemValidator.Validate(filterItem).IsValid)
        {
            throw new InvalidDataException("Invalid filter item name");
        }

        var events = getEventsWithPaginationUseCase.Execute(page);

        var filterBy = GetFilterItem(filterItem, filterValue);
        return events.Where(filterBy);

    }

    private Func<GetEventsResponseDTO, bool> GetFilterItem(string filter, string filterValue)
    {
        switch (filter.ToLower())
        {
            case PLACEFILTER:
                return ev => ev.Place == filterValue;
            case MAXMEMBERSFILTER:
                return ev => ev.MaxMembers == int.Parse(filterValue);
            case CATEGORYIDFILTER:
                return ev => ev.CategoryName == filterValue;
            case CREATORIDFILTER:
                return ev => ev.CreatorName == filterValue;

        }
        return ev => ev.Place == filterValue;
    }
}

