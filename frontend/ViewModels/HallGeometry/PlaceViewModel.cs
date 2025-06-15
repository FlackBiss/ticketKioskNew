using CommunityToolkit.Mvvm.ComponentModel;
using Lastik.Models.Cart.Entities;
using Lastik.Models.Schedules.Entities;
using Lastik.Models.Tickets.Entities;
using System.Diagnostics;
using Point = System.Windows.Point;

namespace Lastik.ViewModels.HallGeometry;

public partial class PlaceViewModel:ObservableObject
{
    public Schedule Schedule { get; }
    public List<PriceValue> Prices { get; }
    [ObservableProperty] private bool _isSelected;

    [ObservableProperty] private PriceTicket _ticket;

    [ObservableProperty] private string? _imagePath;

    public PlaceViewModel(
        PriceTicket ticket,
        Models.Geometry.Entities.HallGeometry geometry,
        Schedule schedule,
        List<PriceValue> prices)
    {
        Schedule = schedule;
        Prices = prices;
        _ticket = ticket;
        SetOffsets(geometry);
        if(geometry.PlaceViews?.PlaceMap is null ||
           geometry.PlaceViews?.PlaceViewsDetails is null ||
           Ticket.Place.Uuid is null) return;
        if (!geometry.PlaceViews.PlaceMap.TryGetValue(Ticket.Place.Uuid, out var placeId)) return;
        if (geometry.PlaceViews.PlaceViewsDetails.TryGetValue(placeId[0], out var place))
        {
            _imagePath = place.Path;
        }
    }

    private void SetOffsets(Models.Geometry.Entities.HallGeometry geometry)
    {
        var sectorGeometry = geometry.Geometry?.Sectors?.FirstOrDefault(item => item.Uuid == Ticket.Place.SectorUuid);
        var fragmentGeometry = sectorGeometry?.Fragments?.FirstOrDefault(item => item.Uuid == Ticket.Place.FragmentUuid);
        var rowGeometry = fragmentGeometry?.Rows?.FirstOrDefault(item => item.Uuid == Ticket.Place.RowUuid);

        var sectorOffset = new Point(sectorGeometry?.Bb?.X??0, sectorGeometry?.Bb?.Y ?? 0);
        var fragmentOffset = new Point(fragmentGeometry?.Bb?.X ?? 0, fragmentGeometry?.Bb?.Y ?? 0);
        var rowOffset = new Point(rowGeometry?.Bb?.X ?? 0, rowGeometry?.Bb?.Y ?? 0);

        var totalOffset = new Point(sectorOffset.X + fragmentOffset.X + rowOffset.X,
            sectorOffset.Y + fragmentOffset.Y + rowOffset.Y);
        if (Ticket.Place.Geometry == null) return;
        Ticket.Place.Geometry.Cx += totalOffset.X;
        Ticket.Place.Geometry.Cy += totalOffset.Y;
    }


}

public static class PlaceViewModelExtensions
{
    public static CartPreview ToCartPreview(this IEnumerable<PlaceViewModel> places) =>
        new()
        {
            Items = places.Select(ToCartPreviewItem).ToList()
        };

    public static CartPreviewItem ToCartPreviewItem(this PlaceViewModel place) => new()
    {
        ItemUuid = place.Ticket.Uuid,
        Price = place.Ticket.Price,
        PriceValues = place.Prices,
        Schedule = place.Schedule.Uuid,
        PriceUuid = place.Ticket.Price.Uuid,
        Quantity = 1
    };
}