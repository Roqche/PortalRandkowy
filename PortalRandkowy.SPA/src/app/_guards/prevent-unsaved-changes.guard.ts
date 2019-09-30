import { CanDeactivate } from '@angular/router';
import { UserEditComponent } from '../users/user-edit/user-edit.component';

export class PreventUnsavedChanges implements CanDeactivate<UserEditComponent> {
    canDeactivate(component: UserEditComponent) {
        if (component.editForm.dirty) {
            return confirm('Jesteś pewien, że chcesz kontynuować? Wszelkie niezapisane zmiany zostaną utracone.');
        }
        return true;
    }
}
