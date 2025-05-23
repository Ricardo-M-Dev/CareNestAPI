﻿using Application.Common.Interfaces;
using Application.Interfaces.Repositories;
using System.Data;

namespace Infrastructure.Persistence
{
    public class UnityOfWork : IUnityOfWork
    {
        private readonly IDbConnection? _connection;
        private readonly IDbTransaction? _transaction;

        public IPersonRepository Persons { get; }
        public IPatientRepository Patients { get; }
        public IPsychologistRepository Psychologists { get; }

        public UnityOfWork(IDbConnection connection, IPersonRepository personRepository, IPatientRepository patients, IPsychologistRepository psychologists)
        {
            _connection = connection;
            _transaction = _connection.BeginTransaction();

            Persons = personRepository;
            Patients = patients;
            Psychologists = psychologists;
        }

        public async Task CommitAsync()
        {
            _transaction?.Commit();
            await Task.CompletedTask;
        }

        public async Task RollbackAsync()
        {
            _transaction?.Rollback();
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}
