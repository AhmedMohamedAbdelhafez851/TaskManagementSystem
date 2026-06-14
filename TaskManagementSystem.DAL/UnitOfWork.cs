//using System;
//using System.Data.SqlClient;
//using TaskManagementSystem.DAL.Abstraction;
//using TaskManagementSystem.DAL.Helpers;
//using TaskManagementSystem.DAL.Repositories;

//namespace TaskManagementSystem.DAL
//{
//    public class UnitOfWork : IUnitOfWork
//    {
//        private SqlConnection _connection;
//        private SqlTransaction _transaction;
//        private bool _disposed;

//        private ITaskRepository _taskRepository;
//        private IUserRepository _userRepository;

//        public UnitOfWork()
//        {
//            _connection = new SqlConnection(DbConnectionFactory.GetConnectionString());
//            _connection.Open();
//            _transaction = _connection.BeginTransaction();
//        }

//        public ITaskRepository Tasks
//        {
//            get
//            {
//                if (_taskRepository == null)
//                {
//                    _taskRepository = new TaskRepository(_connection, _transaction);
//                }
//                return _taskRepository;
//            }
//        }

//        public IUserRepository Users
//        {
//            get
//            {
//                if (_userRepository == null)
//                {
//                    _userRepository = new UserRepository(_connection, _transaction);
//                }
//                return _userRepository;
//            }
//        }

//        public int Commit()
//        {
//            try
//            {
//                _transaction?.Commit();
//                return 1;
//            }
//            catch
//            {
//                _transaction?.Rollback();
//                throw;
//            }
//            finally
//            {
//                _transaction?.Dispose();
//                _transaction = null;
//            }
//        }

//        public void Rollback()
//        {
//            try
//            {
//                _transaction?.Rollback();
//            }
//            finally
//            {
//                _transaction?.Dispose();
//                _transaction = null;
//            }
//        }

//        public void Dispose()
//        {
//            if (!_disposed)
//            {
//                if (_transaction != null)
//                {
//                    _transaction.Dispose();
//                    _transaction = null;
//                }

//                if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
//                {
//                    _connection.Close();
//                    _connection.Dispose();
//                    _connection = null;
//                }

//                _disposed = true;
//            }
//        }

//        public int Complete()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}