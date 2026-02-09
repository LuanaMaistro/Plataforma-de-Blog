import { IPostRepository } from '@/domain/interfaces/IPostRepository';

export class GetPostUseCase {
  constructor(private postRepository: IPostRepository) {}

  async execute(id: number) {
    return await this.postRepository.obterPorId(id);
  }
}
