declare module 'rc-steps' {
    export default class Steps extends React.Component<{current: number}> {
        static Step = class Step extends React.Component<{title: string}> {}
    }
}