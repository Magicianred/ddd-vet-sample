﻿using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Patient;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Specifications;
using FrontDesk.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FrontDesk.Api.PatientEndpoints
{
    public class List : BaseAsyncEndpoint<ListPatientRequest, ListPatientResponse>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public List(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("api/patients")]
        [SwaggerOperation(
            Summary = "List Patients",
            Description = "List Patients",
            OperationId = "patients.List",
            Tags = new[] { "PatientEndpoints" })
        ]
        public override async Task<ActionResult<ListPatientResponse>> HandleAsync([FromQuery] ListPatientRequest request, CancellationToken cancellationToken)
        {
            var response = new ListPatientResponse(request.CorrelationId());

            var patientSpec = new PatientIncludeClientSpecification();
            var patients = await _repository.ListAsync<Patient, int>(patientSpec);
            if (patients is null) return NotFound();

            response.Patients = _mapper.Map<List<PatientDto>>(patients);
            response.Count = response.Patients.Count;

            return Ok(response);
        }
    }
}