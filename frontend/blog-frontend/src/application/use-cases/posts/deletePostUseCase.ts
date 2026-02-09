import { IPostRepository } from '@/domain/interfaces/IPostRepository';

export class DeletePostUseCase {
  constructor(private postRepository: IPostRepository) {}

  async execute(id: number) {
    await this.postRepository.deletar(id);
  }
}
