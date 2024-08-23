using BTL_Car.Data;
using Microsoft.AspNetCore.Mvc;

namespace BTL_Car.Controllers;
[Route("Comments")]
public class CommentsController : Controller {
    public readonly AppDbContext _dbContext;
    public CommentsController(AppDbContext dbContext){
        _dbContext = dbContext;
    }

    public IActionResult DanhSachComment(){
        var cmt = _dbContext.Comments.ToList();
        return View(cmt);
    }
    
    [HttpGet]
    [Route("Delete/{id}")]
    public IActionResult Delete(int id)
    {
        var cmt = _dbContext.Comments.Find(id);
        if (cmt == null)
        {
            return NotFound();
        }

        _dbContext.Comments.Remove(cmt);
        _dbContext.SaveChanges();
        return RedirectToAction("DanhSachComment");
    }
}