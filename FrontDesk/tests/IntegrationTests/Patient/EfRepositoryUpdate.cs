﻿using FrontDesk.Infrastructure.Data;
using System.Threading.Tasks;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Patient
{
    public class EfRepositoryUpdate : BaseEfRepoTestFixture
    {
        private readonly EfRepository _repository;

        public EfRepositoryUpdate()
        {
            _repository = GetRepository();
        }

        [Fact]
        public async Task UpdatesPatientAfterAddingIt()
        {
            var id = 2;
            var name = "changed";

            var patient = await AddPatient(id);

            patient.UpdateName(name);
            await _repository.UpdateAsync<FrontDesk.Core.Aggregates.Patient, int>(patient);

            var updatedPatient = await _repository.GetByIdAsync<FrontDesk.Core.Aggregates.Patient, int>(id);

            Assert.Equal(name, updatedPatient.Name);
        }

        private async Task<FrontDesk.Core.Aggregates.Patient> AddPatient(int id)
        {
            var patient = new PatientBuilder().Id(id).Build();

            await _repository.AddAsync<FrontDesk.Core.Aggregates.Patient, int>(patient);

            return patient;
        }
    }
}
