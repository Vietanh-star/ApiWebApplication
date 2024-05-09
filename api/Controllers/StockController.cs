using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() {
            // return all info of list stocks in StockTable
            var stock = _context.StockTable.ToList()
            .Select(s => s.ToStockDto());
            return Ok(stock);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id) {
            // return info of specific Stock by Id
            var stock = _context.StockTable.Find(id).ToStockDto();
            if(stock == null) {
                return NotFound();
            }

            return Ok(stock);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockDtoRequest stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            _context.StockTable.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockDtoRequest updateDto)
        {
            var stockModel = _context.StockTable.FirstOrDefault(x => x.Id == id);

            if(stockModel == null)
            {
                return NotFound();
            }
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Industry = updateDto.Industry;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Purchase = stockModel.Purchase;
            stockModel.MarketCap = stockModel.MarketCap;

            _context.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stockModel = _context.StockTable.FirstOrDefault(x => x.Id == id);
            if(stockModel == null)
            {
                return NotFound();
            }
            _context.StockTable.Remove(stockModel);
            _context.SaveChanges();
            return NoContent();
        }
    }
}