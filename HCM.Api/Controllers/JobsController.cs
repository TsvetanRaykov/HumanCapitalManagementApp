using AutoMapper;
using HCM.Api.Data.Models;
using HCM.Shared.Data.Contracts;
using HCM.Shared.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HCM.Api.Controllers;

public class JobsController : BaseController
{
    private readonly IRepository<Job> _jobRepository;
    private readonly IMapper _mapper;
    private const string JobNotFountMessage = "Job with id: {0} not found";

    public JobsController(IRepository<Job> jobRepository, IMapper mapper)
    {
        _jobRepository = jobRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllJobs()
    {
        var jobs = await _mapper.ProjectTo<JobDto>(_jobRepository.AllAsNoTracking())
            .ToArrayAsync();

        return Ok(jobs);
    }

    //[HttpGet("{id:int}")]
    //public async Task<IActionResult> GetJobById(int id)
    //{
    //    var job = await _jobRepository.All().FirstOrDefaultAsync(j => j.Id == id);
    //    if (job == null)
    //        return NotFound(string.Format(JobNotFountMessage, id));

    //    return Ok(_mapper.Map<JobDto>(job));
    //}

    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] JobDto job)
    {
        var newJob = _mapper.Map<Job>(job);

        await _jobRepository.AddAsync(newJob);
        await _jobRepository.SaveChangesAsync();

        return Ok(newJob.Id);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateJob([FromBody] JobDto job)
    {
        var jobToUpdate = await _jobRepository.All().FirstOrDefaultAsync(j => j.Id == job.Id);

        if (jobToUpdate == null)
            return NotFound(string.Format(JobNotFountMessage, job.Id));

        _mapper.Map(job, jobToUpdate);

        _jobRepository.Update(jobToUpdate);

        await _jobRepository.SaveChangesAsync();

        return Ok(_mapper.Map<JobDto>(jobToUpdate));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var jobToDelete = await _jobRepository.All().FirstOrDefaultAsync(j => j.Id == id);

        if (jobToDelete == null)
            return NotFound(string.Format(JobNotFountMessage, id));

        _jobRepository.Delete(jobToDelete);
        await _jobRepository.SaveChangesAsync();

        return Ok(_mapper.Map<JobDto>(jobToDelete));
    }


}