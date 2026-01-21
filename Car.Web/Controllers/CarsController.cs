using Car.Core.Dto;
using Car.Core.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace Car.Web.Controllers;

public sealed class CarsController : Controller
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cars = await _carService.GetAllAsync();
        return View(cars);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var car = await _carService.GetAsync(id);
        if (car == null)
            return NotFound();

        return View(car);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CarCreateUpdateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CarCreateUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            var created = await _carService.AddAsync(dto);
            return RedirectToAction(nameof(Details), new { id = created.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var car = await _carService.GetAsync(id);
        if (car == null)
            return NotFound();

        var dto = new CarCreateUpdateDto
        {
            Make = car.Make,
            Model = car.Model,
            Year = car.Year,
            Vin = car.Vin,
            MileageKm = car.MileageKm
        };

        ViewBag.CarId = id;
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CarCreateUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.CarId = id;
            return View(dto);
        }

        try
        {
            await _carService.UpdateAsync(id, dto);
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            ViewBag.CarId = id;
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var car = await _carService.GetAsync(id);
        if (car == null)
            return NotFound();

        return View(car);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _carService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
