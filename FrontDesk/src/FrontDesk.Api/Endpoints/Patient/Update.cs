﻿using System.Threading;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using BlazorShared.Models.Patient;
using FrontDesk.SharedKernel.Interfaces;
using AutoMapper;
using FrontDesk.Core.Aggregates;

namespace FrontDesk.Api.PatientEndpoints
{
    public class Update : BaseAsyncEndpoint<UpdatePatientRequest, UpdatePatientResponse>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public Update(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPut("api/patients")]
        [SwaggerOperation(
            Summary = "Updates a Patient",
            Description = "Updates a Patient",
            OperationId = "patients.update",
            Tags = new[] { "PatientEndpoints" })
        ]
        public override async Task<ActionResult<UpdatePatientResponse>> HandleAsync(UpdatePatientRequest request, CancellationToken cancellationToken)
        {
            var response = new UpdatePatientResponse(request.CorrelationId());

            var toUpdate = _mapper.Map<Patient>(request);
            await _repository.UpdateAsync<Patient, int>(toUpdate);

            var dto = _mapper.Map<PatientDto>(toUpdate);
            response.Patient = dto;

            return Ok(response);
        }
    }
}
