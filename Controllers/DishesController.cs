using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class DishesController : Controller
{
    private readonly AppDb _db;
    private readonly Lang L;
    public DishesController(AppDb db, Lang l) { _db = db; L = l; }

    public async Task<IActionResult> Index(string? q, int? categoryId, string? status,
        decimal? minPrice, decimal? maxPrice, string? sort)
    {
        // Khoảng giá không hợp lệ
        if (minPrice < 0 || maxPrice < 0 || (minPrice != null && maxPrice != null && minPrice > maxPrice))
        {
            ViewBag.Error = L["Err_Range"];
            minPrice = null; maxPrice = null;
        }

        // Toàn bộ lọc/sắp xếp chạy ở tầng database (IQueryable)
        var query = _db.Dishes.Include(d => d.DishCategory).Include(d => d.Images).AsQueryable();

        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(d => d.Name.Contains(q));
        if (categoryId != null) query = query.Where(d => d.DishCategoryId == categoryId);
        if (status == "available") query = query.Where(d => d.IsAvailable);
        else if (status == "unavailable") query = query.Where(d => !d.IsAvailable);
        if (minPrice != null) query = query.Where(d => d.Price >= minPrice);
        if (maxPrice != null) query = query.Where(d => d.Price <= maxPrice);

        query = sort switch
        {
            "price_asc" => query.OrderBy(d => d.Price),
            "price_desc" => query.OrderByDescending(d => d.Price),
            "prep_asc" => query.OrderBy(d => d.PreparationTime),
            _ => query.OrderBy(d => d.Id)
        };

        ViewBag.Categories = new SelectList(await _db.DishCategories.ToListAsync(), "Id", "Name", categoryId);
        ViewBag.Q = q; ViewBag.Status = status; ViewBag.Sort = sort;
        ViewBag.MinPrice = minPrice; ViewBag.MaxPrice = maxPrice;

        return View(await query.ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var d = await _db.Dishes.Include(x => x.DishCategory).Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id);
        return d == null ? NotFoundPage() : View(d);
    }

    // ===== Create / Edit =====
    public async Task<IActionResult> Create()
    {
        await LoadCats();
        return View(new Dish_BIT247667());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Dish_BIT247667 d)
    {
        if (!await Validate(d)) { await LoadCats(d.DishCategoryId); return View(d); }
        _db.Dishes.Add(d);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var d = await _db.Dishes.FindAsync(id);
        if (d == null) return NotFoundPage();
        await LoadCats(d.DishCategoryId);
        return View(d);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Dish_BIT247667 d)
    {
        if (id != d.Id || !await _db.Dishes.AnyAsync(x => x.Id == id)) return NotFoundPage();
        if (!await Validate(d)) { await LoadCats(d.DishCategoryId); return View(d); }
        _db.Dishes.Update(d);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // ===== Ảnh =====
    [HttpPost]
    public async Task<IActionResult> AddImage(int dishId, string? imageUrl, bool isThumbnail)
    {
        if (!await _db.Dishes.AnyAsync(d => d.Id == dishId)) return NotFoundPage();
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            TempData["Err"] = L["Err_Required"];
            return RedirectToAction(nameof(Details), new { id = dishId });
        }
        if (isThumbnail) await ClearThumb(dishId);
        _db.DishImages.Add(new DishImage_BIT247667 { DishId = dishId, ImageUrl = imageUrl.Trim(), IsThumbnail = isThumbnail });
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = dishId });
    }

    [HttpPost]
    public async Task<IActionResult> SetThumbnail(int imageId)
    {
        var img = await _db.DishImages.FindAsync(imageId);
        if (img == null) return NotFoundPage();
        await ClearThumb(img.DishId);
        img.IsThumbnail = true;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = img.DishId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteImage(int imageId)
    {
        var img = await _db.DishImages.FindAsync(imageId);
        if (img == null) return NotFoundPage();
        _db.DishImages.Remove(img);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = img.DishId });
    }

    // ===== Helpers =====
    async Task LoadCats(int? selected = null) =>
        ViewBag.Categories = new SelectList(await _db.DishCategories.ToListAsync(), "Id", "Name", selected);

    IActionResult NotFoundPage() { Response.StatusCode = 404; return View("NotFound"); }

    async Task ClearThumb(int dishId)
    {
        var list = await _db.DishImages.Where(i => i.DishId == dishId && i.IsThumbnail).ToListAsync();
        foreach (var i in list) i.IsThumbnail = false;
    }

    async Task<bool> Validate(Dish_BIT247667 d)
    {
        if (!await _db.DishCategories.AnyAsync(c => c.Id == d.DishCategoryId))
            ModelState.AddModelError("DishCategoryId", "Err_Cat");
        else if (!string.IsNullOrWhiteSpace(d.Name) &&
                 await _db.Dishes.AnyAsync(x => x.DishCategoryId == d.DishCategoryId && x.Name == d.Name && x.Id != d.Id))
            ModelState.AddModelError("Name", "Err_Dup");

        L.Translate(ModelState);
        return ModelState.IsValid;
    }
}