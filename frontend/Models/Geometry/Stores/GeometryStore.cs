using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lastik.Helpers.Logging;
using Lastik.Models.Geometry.Entities;
using Lastik.Models.Schedules.Entities;

namespace Lastik.Models.Geometry.Stores;

public class GeometryStore(IApiHttpClient httpClient,ILoggingService logger)
{
    public async Task<Schedule> GetAsync(int hallGeometryId) =>
        await LoadImages(
            (await httpClient.GetHallGeometry(hallGeometryId)).GetContent(logger));

    private async Task<Schedule> LoadImages(Schedule geometry)
    {
        if (geometry.Hall?.ImageUri is null) return geometry;
        geometry.Hall.ImagePath = await httpClient.LoadImageAndGetPath(logger, geometry.Hall.ImageUri);

        return geometry;
    }
}