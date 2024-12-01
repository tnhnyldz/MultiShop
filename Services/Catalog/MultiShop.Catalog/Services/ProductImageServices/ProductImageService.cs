using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductImageDtos;
using MultiShop.Catalog.Entites;

namespace MultiShop.Catalog.Services.ProductImageServices
{
	public class ProductImageService : IProductImageService
	{
		private readonly IMongoCollection<ProductImage> _productImageCollection;
		private readonly IMapper _mapper;

		public ProductImageService(IMapper mapper, IConfiguration configuration)
		{
			var connectionString = configuration.GetSection("DatabaseSettings:ConnectionString").Value;
			var databaseName = configuration.GetSection("DatabaseSettings:DatabaseName").Value;
			var productImageCollectionName = configuration.GetSection("DatabaseSettings:ProductImageCollectionName").Value;

			var client = new MongoClient(connectionString);
			var database = client.GetDatabase(databaseName);

			_productImageCollection = database.GetCollection<ProductImage>(productImageCollectionName);
			_mapper = mapper;
		}

		public async Task CreateProductImageAsync(CreateProductImageDto createProductImageDto)
		{
			var value = _mapper.Map<ProductImage>(createProductImageDto);
			await _productImageCollection.InsertOneAsync(value);
		}

		public async Task DeleteProductImageAsync(string id)
		{
			await _productImageCollection.DeleteOneAsync(x => x.ProductImageId == id);
		}

		public async Task<GetByIdProductImageDto> GetByIdProductImageAsync(string id)
		{
			var values = await _productImageCollection.Find<ProductImage>(x => x.ProductImageId == id).FirstOrDefaultAsync();
			return _mapper.Map<GetByIdProductImageDto>(values);
		}

		public async Task<GetByIdProductImageDto> GetByProductIdProductImageAsync(string id)
		{
			var values = await _productImageCollection.Find(x => x.ProductId == id).FirstOrDefaultAsync();
			return _mapper.Map<GetByIdProductImageDto>(values);
		}

		public async Task<List<ResultProductImageDto>> GettAllProductImageAsync()
		{
			var values = await _productImageCollection.Find(x => true).ToListAsync();
			return _mapper.Map<List<ResultProductImageDto>>(values);
		}

		public async Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto)
		{
			var values = _mapper.Map<ProductImage>(updateProductImageDto);
			await _productImageCollection.FindOneAndReplaceAsync(x => x.ProductImageId == updateProductImageDto.ProductImageId, values);
		}
	}
}