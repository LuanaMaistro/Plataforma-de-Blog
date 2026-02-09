import { IAuthRepository } from '@/domain/interfaces/IAuthRepository';
import { registerValidator } from '@/application/validators/registerValidator';
import { RegisterDto } from '@/application/dtos/RegisterDto';

export class RegisterUseCase {
  constructor(private authRepository: IAuthRepository) {}

  async execute(dto: RegisterDto) {
    const validation = registerValidator.safeParse(dto);
    if (!validation.success) {
      throw new Error(validation.error.issues[0].message);
    }

    await this.authRepository.register(dto.nome, dto.email, dto.senha);
  }
}
