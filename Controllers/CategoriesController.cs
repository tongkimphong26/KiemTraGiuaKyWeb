using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CategoriesController : Controller
{
    private readonly AppDb _db;
    private readonly Lang L;
    public CategoriesController(AppDb db, Lang l) { _db = db; L = l; }

    public async Task<IActionResult> Index() =>
        View(await _db.DishCategories.Include(c => c.Dishes).OrderBy(c => c.Id).ToListAsync());

    public IActionResult Create() => View(new DishCategory_BIT247667());

    [HttpPost]
    public async Task<IActionResult> Create(DishCategory_BIT247667 c)
    {
        if (!ModelState.IsValid) { L.Translate(ModelState); return View(c); }
        _db.DishCategories.Add(c);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var c = await _db.DishCategories.FindAsync(id);
        return c == null ? NotFoundPage() : View(c);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, DishCategory_BIT247667 c)
    {
        if (id != c.Id || !await _db.DishCategories.AnyAsync(x => x.Id == id)) return NotFoundPage();
        if (!ModelState.IsValid) { L.Translate(ModelState); return View(c); }
        _db.DishCategories.Update(c);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Không cho xóa loại món đang có món sử dụng
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.DishCategories.FindAsync(id);
        if (c == null) return NotFoundPage();
        if (await _db.Dishes.AnyAsync(d => d.DishCategoryId == id))
        {
            TempData["Err"] = L["Err_CatInUse"];
            return RedirectToAction(nameof(Index));
        }
        _db.DishCategories.Remove(c);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    IActionResult NotFoundPage() { Response.StatusCode = 404; return View("NotFound"); }
}