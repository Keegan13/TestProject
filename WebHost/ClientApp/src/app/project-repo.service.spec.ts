import { TestBed } from '@angular/core/testing';

import { ProjectRepoService } from './project-repo.service';

describe('ProjectRepoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProjectRepoService = TestBed.get(ProjectRepoService);
    expect(service).toBeTruthy();
  });
});
