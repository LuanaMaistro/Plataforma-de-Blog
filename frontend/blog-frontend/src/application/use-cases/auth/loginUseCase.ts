import { IAuthRepository } from '@/domain/interfaces/IAuthRepository';
import { loginValidator } from '@/application/validators/loginValidator';
import { LoginDto } from '@/application/dtos/LoginDto';

export class LoginUseCase {
  constructor(private authRepository: IAuthRepository) {}

  async execute(dto: LoginDto) {
    const validation = loginValidator.safeParse(dto);
    if (!validation.success) {
      throw new Error(validation.error.issues[0].message);
    }

    return await this.authRepository.login(dto.email, dto.senha);
  }
}
