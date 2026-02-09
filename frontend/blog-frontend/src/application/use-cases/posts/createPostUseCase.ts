import { IPostRepository } from '@/domain/interfaces/IPostRepository';
import { CreatePostDto } from '@/application/dtos/PostDto';
import { postValidator } from '@/application/validators/postValidator';

export class CreatePostUseCase {
  constructor(private postRepository: IPostRepository) {}

  async execute(dto: CreatePostDto) {
    const validation = postValidator.safeParse(dto);
    if (!validation.success) {
      throw new Error(validation.error.issues[0].message);
    }

    return await this.postRepository.criar(dto.titulo, dto.conteudo);
  }
}
