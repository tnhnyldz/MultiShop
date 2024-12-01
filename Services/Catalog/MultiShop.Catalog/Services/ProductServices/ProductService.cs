using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductDtos;
using MultiShop.Catalog.Entites;

namespace MultiShop.Catalog.Services.ProductServices
{
	public class ProductService : IProductService
	{
		private readonly IMapper _mapper;
		private readonly IMongoCollection<Product> _productCollection;
		public ProductService(IMapper mapper, IConfiguration configuration)
		{
			var connectionString = configuration.GetSection("DatabaseSettings:ConnectionString").Value;
			var databaseName = configuration.GetSection("DatabaseSettings:DatabaseName").Value;
			var productCollectionName = configuration.GetSection("DatabaseSettings:ProductCollectionName").Value;

			var client = new MongoClient(connectionString);
			var database = client.GetDatabase(databaseName);

			_productCollection = database.GetCollection<Product>(productCollectionName);
			_mapper = mapper;
		}
		public async Task CreateProductAsync(CreateProductDto createProductDto)
		{
			var values = _mapper.Map<Product>(createProductDto);
			await _productCollection.InsertOneAsync(values);
		}
		public async Task DeleteProductAsync(string id)
		{
			await _productCollection.DeleteOneAsync(x => x.ProductId == id);
		}
		public async Task<GetByIdProductDto> GetByIdProductAsync(string id)
		{
			var values = await _productCollection.Find<Product>(x => x.ProductId == id).FirstOrDefaultAsync();
			return _mapper.Map<GetByIdProductDto>(values);
		}


		public async Task<List<ResultProductDto>> GettAllProductAsync()
		{
			var values = await _productCollection.Find(x => true).ToListAsync();
			return _mapper.Map<List<ResultProductDto>>(values);
		}
		public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
		{
			var values = _mapper.Map<Product>(updateProductDto);
			await _productCollection.FindOneAndReplaceAsync(x => x.ProductId == updateProductDto.ProductId, values);
		}
	}
}
