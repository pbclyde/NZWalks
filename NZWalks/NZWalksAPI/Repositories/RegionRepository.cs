using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nZWalksDbContext.Regions.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await nZWalksDbContext.Regions.FirstOrDefaultAsync(dataRow => dataRow.Id == id);
            if (region == null)
            {
                return null;
            }
            // Delete region
            nZWalksDbContext.Regions.Remove(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Regions.FirstOrDefaultAsync(dataRow => dataRow.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var regionFromDB = await nZWalksDbContext.Regions.FirstOrDefaultAsync(dataRow => dataRow.Id == id);
            if (regionFromDB == null)
            {
                return null;
            }
            regionFromDB.Code = region.Code;
            regionFromDB.Name= region.Name;
            regionFromDB.Area = region.Area;
            regionFromDB.Lat= region.Lat;
            regionFromDB.Long   = region.Long;  
            regionFromDB.Population = region.Population;
            await nZWalksDbContext.SaveChangesAsync();
            return regionFromDB;
        }
    }
}
