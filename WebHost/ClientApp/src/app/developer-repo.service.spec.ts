import { TestBed } from '@angular/core/testing';

import { DeveloperRepoService } from './developer-repo.service';

describe('DeveloperRepoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DeveloperRepoService = TestBed.get(DeveloperRepoService);
    expect(service).toBeTruthy();
  });
});
