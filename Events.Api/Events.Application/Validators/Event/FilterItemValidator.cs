using FluentValidation;

namespace Events.Application.Validators.Event
{
    internal class FilterItemValidator : AbstractValidator<string>
    {
        private const string PLACEFILTER = "place";
        private const string MAXMEMBERSFILTER = "maxmembers";
        private const string CATEGORYIDFILTER = "categoryid";
        private const string CREATORIDFILTER = "creatorid";

        public FilterItemValidator()
        {
            RuleFor((item) => item.ToLower() == PLACEFILTER
            || item.ToLower() == MAXMEMBERSFILTER
            || item.ToLower() == CATEGORYIDFILTER
            || item.ToLower() == CREATORIDFILTER);
        }
    }
}
