import { BaseEvent } from "./BaseEvent";
export class EditableEvent extends BaseEvent<boolean> {
  constructor() {
    super("EditableEvent");
  }
}
