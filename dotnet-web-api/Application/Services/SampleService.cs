﻿using Application.Interfaces;
using Application.Services.BaseService;
using CoreKit.EventHandler.Interfaces;
using Database;
using StarterApp.Domain.Model;

namespace Application.Services;

public class SampleService(AppDbContext dbContext, IEventHandler eventHandler) : BaseCrudService<SampleEntity, long>(dbContext, eventHandler), ISampleService
{
}
