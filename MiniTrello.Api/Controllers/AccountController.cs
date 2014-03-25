using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AttributeRouting.Web.Http;
using AutoMapper;
using MiniTrello.Api.Controllers.Helpers;
using MiniTrello.Api.CustomExceptions;
using MiniTrello.Api.Models;
using MiniTrello.Domain.DataObjects;
using MiniTrello.Domain.Entities;
using MiniTrello.Domain.Services;

namespace MiniTrello.Api.Controllers
{
    public class AccountController : ApiController
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        readonly IMappingEngine _mappingEngine;
        readonly IRegisterValidator<AccountRegisterModel> _registerValidator;

        public AccountController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository,
            IMappingEngine mappingEngine, IRegisterValidator<AccountRegisterModel> registerValidator)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _mappingEngine = mappingEngine;
            _registerValidator = registerValidator;
        }

        [HttpPost]
        [POST("login")]
        public AuthenticationModel Login([FromBody] AccountLoginModel model)
        {
            //var account = _readOnlyRepository.First<Account>(account1 => account1.Email == model.Email 
            //    && BCrypt.Net.BCrypt.Verify(model.Password, account1.Password));
            var account = _readOnlyRepository.First<Account>(account1 => account1.Email == model.Email
                && account1.Password == model.Password);

            if (account != null)
            {
                var session = AccountHelpers.CreateNewSession(account);
                var sessionCreated = _writeOnlyRepository.Create(session);
                if (sessionCreated != null)
                    return new AuthenticationModel()
                    {
                        Token = session.Token,
                        YourSessionExpireIn = session.Duration
                    };
            }
            throw new BadRequestException("User or Password is incorrect");
        }

        [AcceptVerbs("POST")]
        [POST("register")]
        public SuccessfulMessageResponse Register([FromBody] AccountRegisterModel model)
        {
            if (AccountHelpers.IsAValidRegister(model))
            {
                //string passwordEncode = BCrypt.Net.BCrypt.HashPassword(model.Password);
                var account = _mappingEngine.Map<AccountRegisterModel, Account>(model);
                account.IsArchived = false;
                //account.Password = passwordEncode;
                account.Password = model.Password;
                Account accountCreated = _writeOnlyRepository.Create(account);
                if (accountCreated != null)
                {
                    AccountHelpers.SendMessage(model.Email, model.FirstName + " " + model.LastName, 1);
                    AccountHelpers.CreateOrganization(accountCreated);
                    return new SuccessfulMessageResponse("You have been registered succesfully");
                }
            }
            throw new BadRequestException("The Account couldn't be created");
        }
    }
}