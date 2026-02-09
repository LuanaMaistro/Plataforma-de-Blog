import { IPostRepository } from '@/domain/interfaces/IPostRepository';
import { UpdatePostDto } from '@/application/dtos/PostDto';
import { postValidator } from '@/application/validators/postValidator';

export class UpdatePostUseCase {
  constructor(private postRepository: IPostRepository) {}

  async execute(id: number, dto: UpdatePostDto) {
    const validation = postValidator.safeParse(dto);
    if (!validation.success) {
      throw new Error(validation.error.issues[0].message);
    }

    return await this.postRepository.atualizar(id, dto.titulo, dto.conteudo);
  }
}
