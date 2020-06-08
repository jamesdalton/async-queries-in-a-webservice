using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Interfaces;
using Api.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Api.DataAccess
{
    public class AnimeDataAccess: IAnimeDataAccess
    {
        private IConfiguration configuration;

        public AnimeDataAccess(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IEnumerable<int> Get()
        {
            using (var connection = new NpgsqlConnection(configuration["connectionString"]))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id from anime";
                    using (var reader = command.ExecuteReader())
                    {
                        var result = new List<int>();
                        while (reader.Read())
                        {
                            result.Add(reader.GetInt32(0));
                        }
                        return result;
                    }
                }
            }
        }

        public Anime Get(int id)
        {
            using (var connection = new NpgsqlConnection(configuration["connectionString"]))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var result = new Anime();
                    command.CommandText = "select anime.id, anime.name, type.name, episodes, rating, members from anime join type on type_id = type.id where anime.id = @id";
                    command.Parameters.Add(new NpgsqlParameter("id", id));
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Id = reader.GetInt32(0);
                            result.Name = reader.GetString(1);
                            result.Type = reader.GetString(2);
                            result.Episodes = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                            result.Rating = reader.IsDBNull(4) ? (double?)null : reader.GetDouble(4);
                            result.Members = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5);
                        }
                    }
                    result.Genres = new List<string>();
                    command.CommandText = "select genre.name from anime_genre join genre on anime_genre.genre_id = genre.id where anime_id = @id";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Genres.Add(reader.GetString(0));
                        }
                    }
                    return result;
                }
            }
        }

        public async Task<Anime> GetAsync(int id)
        {
            using (var connection = new NpgsqlConnection(configuration["connectionString"]))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    var result = new Anime();
                    command.CommandText = "select anime.id, anime.name, type.name, episodes, rating, members from anime join type on type_id = type.id where anime.id = @id";
                    command.Parameters.Add(new NpgsqlParameter("id", id));
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result.Id = reader.GetInt32(0);
                            result.Name = reader.GetString(1);
                            result.Type = reader.GetString(2);
                            result.Episodes = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                            result.Rating = reader.IsDBNull(4) ? (double?)null : reader.GetDouble(4);
                            result.Members = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5);
                        }
                    }
                    result.Genres = new List<string>();
                    command.CommandText = "select genre.name from anime_genre join genre on anime_genre.genre_id = genre.id where anime_id = @id";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Genres.Add(reader.GetString(0));
                        }
                    }
                    return result;
                }
            }
        }
    }
}