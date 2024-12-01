using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductDetailDtos;
using MultiShop.Catalog.Entites;

namespace MultiShop.Catalog.Services.ProductDetailDetailServices
{
	public class ProductDetailService : IProductDetailService
	{
		private readonly IMapper _mapper;
		private readonly IMongoCollection<ProductDetail> _productDetailCollection;

		public ProductDetailService(IMapper mapper, IConfiguration configuration)
		{
			var connectionString = configuration.GetSection("DatabaseSettings:ConnectionString").Value;
			var databaseName = configuration.GetSection("DatabaseSettings:DatabaseName").Value;
			var productDetailCollectionName = configuration.GetSection("DatabaseSettings:ProductDetailCollectionName").Value;

			var client = new MongoClient(connectionString);
			var database = client.GetDatabase(databaseName);

			_productDetailCollection = database.GetCollection<ProductDetail>(productDetailCollectionName);
			_mapper = mapper;
		}

		public async Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto)
		{
			var values = _mapper.Map<ProductDetail>(createProductDetailDto);
			await _productDetailCollection.InsertOneAsync(values);
		}

		public async Task DeleteProductDetailAsync(string id)
		{
			await _productDetailCollection.DeleteOneAsync(x => x.ProductDetailId == id);
		}

		public async Task<GetByIdProductDetailDto> GetByIdProductDetailAsync(string id)
		{
			var values = await _productDetailCollection.Find<ProductDetail>(x => x.ProductDetailId == id).FirstOrDefaultAsync();
			return _mapper.Map<GetByIdProductDetailDto>(values);
		}

		public async Task<GetByIdProductDetailDto> GetByProductIdProductDetailAsync(string id)
		{
			var values = await _productDetailCollection.Find<ProductDetail>(x => x.ProductId == id).FirstOrDefaultAsync();
			return _mapper.Map<GetByIdProductDetailDto>(values);
		}

		public async Task<List<ResultProductDetailDto>> GettAllProductDetailAsync()
		{
			var values = await _productDetailCollection.Find(x => true).ToListAsync();
			return _mapper.Map<List<ResultProductDetailDto>>(values);
		}

		public async Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto)
		{
			var values = _mapper.Map<ProductDetail>(updateProductDetailDto);
			await _productDetailCollection.FindOneAndReplaceAsync(x => x.ProductDetailId == updateProductDetailDto.ProductDetailId, values);
		}
	}
}