import { TestBed } from '@angular/core/testing';

import { BaseRepoService } from './base-repo.service';

describe('BaseRepoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BaseRepoService = TestBed.get(BaseRepoService);
    expect(service).toBeTruthy();
  });
});
