import { ServiceModelBase } from './service-model-base';

export abstract class ServiceModelRoot<T> extends ServiceModelBase {
    id!: T;
    createdBy!: string;
    lastModifiedBy!: string;
}
