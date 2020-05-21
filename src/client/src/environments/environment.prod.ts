import { IEnviromnent } from './IEnvironment';
import { LogLevel } from '@microsoft/signalr';

export const environment: IEnviromnent = {
	production: true,
	resources: {
		base: '/resources/images/'
	},
	api: {
		url: '/api'
	},
	signalr: {
		url: '/hubs/game',
		logLevel: LogLevel.Error
	}
};
