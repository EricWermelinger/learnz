import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { UserChangePasswordDTO } from 'src/app/DTOs/User/UserChangePasswordDTO';
import { ApiService } from 'src/app/Framework/API/api.service';
import { FormGroupTyped } from 'src/app/Material/types';
import { PasswordChangeDialogService } from './password-change-dialog.service';

@Component({
  selector: 'app-password-change-dialog',
  templateUrl: './password-change-dialog.component.html',
  styleUrls: ['./password-change-dialog.component.scss']
})
export class PasswordChangeDialogComponent {

  formGroup: FormGroupTyped<UserChangePasswordDTO & { confirmPassword: string }>;

  constructor(
    private passwordService: PasswordChangeDialogService,
    private formBuilder: FormBuilder,
    private dialogRef: MatDialogRef<PasswordChangeDialogComponent>,
  ) {
    this.formGroup = this.formBuilder.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    }) as FormGroupTyped<UserChangePasswordDTO & { confirmPassword: string }>;
    this.formGroup.addValidators(() => {
      if (this.formGroup.value.newPassword !== this.formGroup.value.confirmPassword) {
        return { confirmPassword: true };
      }
      return null;
    });
  }

  save() {
    const value = {
      newPassword: this.formGroup.value.newPassword,
      oldPassword: this.formGroup.value.oldPassword
    } as UserChangePasswordDTO;
    this.passwordService.save(value).subscribe(_ => this.close());
  }

  close() {
    this.dialogRef.close();
  }
}