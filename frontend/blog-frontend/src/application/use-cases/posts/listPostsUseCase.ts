import { IPostRepository } from '@/domain/interfaces/IPostRepository';

export class ListPostsUseCase {
  constructor(private postRepository: IPostRepository) {}

  async execute(apenasMinhas = false) {
    return await this.postRepository.listar(apenasMinhas);
  }
}
