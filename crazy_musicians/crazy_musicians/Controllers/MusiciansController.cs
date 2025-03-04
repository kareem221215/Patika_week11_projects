using crazy_musicians.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/musicians")]
public class MusiciansController : ControllerBase
{
    private static List<Musician> musicians = new List<Musician>
    {
        new Musician { Id = 1, Name = "Ahmet Çalgı", Job = "Ünlü Çalgı Çalar", FunFact = "Her zaman yanlış nota çalar, ama çok eğlenceli" },
        new Musician { Id = 2, Name = "Zeynep Melodi", Job = "Popüler Melodi Yazarı", FunFact = "Şarkıları yanlış anlaşılır ama çok popüler" },
        new Musician { Id = 3, Name = "Cemil Akor", Job = "Çılgın Akorist", FunFact = "Akorları sık değiştirir, ama şaşırtıcı derecede yetenekli" },
        new Musician { Id = 4, Name = "Fatma Nota", Job = "Sürpriz Nota Üreticisi", FunFact = "Nota üretirken sürekli sürprizler hazırlar" },
        new Musician { Id = 5, Name = "Hasan Ritim", Job = "Ritim Canavarı", FunFact = "Her ritmi kendi tarzında yapar, hiç uymaz ama komiktir" },
    };

    [HttpGet]
    public ActionResult<IEnumerable<Musician>> GetAll()
    {
        return Ok(musicians);
    }

    [HttpGet("{id}")]
    public ActionResult<Musician> GetById(int id)
    {
        var musician = musicians.FirstOrDefault(m => m.Id == id);
        if (musician == null) return NotFound();
        return Ok(musician);
    }

    [HttpGet("search")]
    public ActionResult<IEnumerable<Musician>> Search([FromQuery] string name)
    {
        var results = musicians.Where(m => m.Name.ToLower().Contains(name.ToLower())).ToList();
        return Ok(results);
    }

    [HttpPost]
    public ActionResult<Musician> Create([FromBody] Musician newMusician)
    {
        if (string.IsNullOrWhiteSpace(newMusician.Name) || string.IsNullOrWhiteSpace(newMusician.Job))
        {
            return BadRequest("Name and Job are required.");
        }
        newMusician.Id = musicians.Max(m => m.Id) + 1;
        musicians.Add(newMusician);
        return CreatedAtAction(nameof(GetById), new { id = newMusician.Id }, newMusician);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] Musician updatedMusician)
    {
        var musician = musicians.FirstOrDefault(m => m.Id == id);
        if (musician == null) return NotFound();

        musician.Name = updatedMusician.Name;
        musician.Job = updatedMusician.Job;
        musician.FunFact = updatedMusician.FunFact;

        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult PartialUpdate(int id, [FromBody] Dictionary<string, string> updates)
    {
        var musician = musicians.FirstOrDefault(m => m.Id == id);
        if (musician == null) return NotFound();

        if (updates.ContainsKey("Name")) musician.Name = updates["Name"];
        if (updates.ContainsKey("Job")) musician.Job = updates["Job"];
        if (updates.ContainsKey("FunFact")) musician.FunFact = updates["FunFact"];

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var musician = musicians.FirstOrDefault(m => m.Id == id);
        if (musician == null) return NotFound();

        musicians.Remove(musician);
        return NoContent();
    }
}
